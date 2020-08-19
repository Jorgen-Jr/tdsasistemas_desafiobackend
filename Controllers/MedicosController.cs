using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace DesafioBackend.Controllers
{
    [Route("Medico")]
    [ApiController]
    [Authorize]
    public class MedicosController : ControllerBase
    {
        private readonly MedicoContext _context;

        public MedicosController(MedicoContext context)
        {
            _context = context;
        }

        // GET: /Medicos
        /* Buscar e retornar lista com todos os médicos cadastrados no banco de dados com suas especialidades.
         */
        [HttpGet]
        public async Task<IEnumerable<MedicoDTO>> GetMedicos()
        {
            var medicos = await _context.Medicos.Include("especialidades.especialidade").ToListAsync();

            List<MedicoDTO> responseMedicos = new List<MedicoDTO>();

            foreach (Medico med in medicos)
            {
                List<String> especialidades = new List<String>();
                foreach (MedicoEspecialidade especialidade in med.especialidades)
                {
                    especialidades.Add(especialidade.especialidade.nome);
                }
                responseMedicos.Add(new MedicoDTO()
                {
                    id = med.id,
                    nome = med.nome,
                    cpf = med.cpf,
                    crm = med.crm,
                    especialidades = especialidades
                });
            }

            return responseMedicos;
        }

        /* Buscar todas as especialidades encontradas na busca de palavra chave e retornar todos os médicos que a possuem.
         * TODO this can be improved by using a singles query with less foreach blocks.
         */
        // GET: /Medico/Especialidade
        [HttpGet("{query}")]
        public async Task<IEnumerable<MedicoDTO>> GetMedico(String query)
        {
            // Busca das especialidades onde conter a query.
            var targetEspecialidade = await _context.Especialidade
                .Where(especialidade => especialidade.nome.Contains(query))
                .Include("medicoespecialidade")
                .ToListAsync();

            // Busca todos os médicos.
            var medicos = await _context.Medicos
                .Include("especialidades.especialidade")
                .ToListAsync();

            List<MedicoDTO> responseMedicos = new List<MedicoDTO>();

            // Iterar entre as especialidades encontradas na seleção.
            foreach (Especialidade especialidade in targetEspecialidade)
            {
                // Iterar entre os relacionamentos das especialidades encontradas
                foreach (MedicoEspecialidade medicoEspecialidade in especialidade.medicoespecialidade)
                {
                    List<String> especialidades = new List<String>();

                    //Iterar entre as especialidades para adcionar ao objeto de resposta MedicoDTO.
                    foreach (MedicoEspecialidade item in medicoEspecialidade.medico.especialidades)
                    {
                        especialidades.Add(item.especialidade.nome);
                    }

                    responseMedicos.Add(new MedicoDTO()
                    {
                        id = medicoEspecialidade.medico.id,
                        nome = medicoEspecialidade.medico.nome,
                        cpf = medicoEspecialidade.medico.cpf,
                        crm = medicoEspecialidade.medico.crm,
                        especialidades = especialidades,

                    });
                }
            }

            return responseMedicos;
        }

        /* Cadastrar um novo médico no banco de dados, com suas especialidades.
         * Retornar erro ao enviar campos inválidos.
         * Validação através do método checkfields() : List<error>
         */
        // POST: /Medicos
        [HttpPost]
        public async Task<ObjectResult> PostMedico(MedicoDTO medico)
        {
            List<String> especialidades = new List<String>();

            List<error> ModelErrors = checkfields(medico);

            //Se não encontrar nenhum erro proseguir com o cadastro.
            if (ModelErrors.Count() == 0)
            {
                Medico newMedico = new Medico()
                {
                    nome = medico.nome,
                    cpf = medico.cpf,
                    crm = medico.crm
                };

                //Adcionar as especialidades ao objeto Medico.
                foreach (string especialidade in medico.especialidades)
                {
                    Especialidade newEspecialidade = new Especialidade();

                    //Se a especialidade já existir, basta criar a especialidade.
                    if (EspecialidadeExists(especialidade))
                    {
                        var response = _context.Especialidade.Where(item => item.nome == especialidade);

                        newEspecialidade = response.FirstOrDefault();
                    }
                    //Se a especialidade não existir, basta criar a especialidade.
                    else
                    {
                        newEspecialidade = new Especialidade()
                        {
                            nome = especialidade
                        };
                    }

                    // Adcionar a especialidade ao médico.
                    MedicoEspecialidade newMedicoEspecialidade = new MedicoEspecialidade()
                    {
                        medico = newMedico,
                        especialidade = newEspecialidade
                    };

                    _context.MedicosEspecialidades.Add(newMedicoEspecialidade);

                    newMedico.especialidades.Add(newMedicoEspecialidade);
                }

                //Salvar o médico no banco de dados.
                _context.Medicos.Add(newMedico);
                await _context.SaveChangesAsync();

                // Acrescentar seu id para retornar o objeto MedicoDTO.
                medico.id = newMedico.id;
            }

            //Se encontrar erros retornar os erros encontrados com o status 400
            else
            {
                return BadRequest(JsonConvert.SerializeObject(new
                {
                    erros = ModelErrors
                }));
            }

            return Ok(medico);
        }

        /* Remover um registro do banco de dados através do seu id.
         */
        // DELETE: /Medicos/{Guid}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Medico>> DeleteMedico(Guid id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }

            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();

            return medico;
        }

        /* Verificar se a especialidade já existe.
         * Usada na criação de novos médicos, para poder reutilizar se já tiver uma especialidade com o mesmo nome.
         */
        private bool EspecialidadeExists(String nome)
        {
            return _context.Especialidade.Any(e => e.nome == nome);
        }

        /* Verificar se todos os campos são válidos e retornar uma lista com os erros encontrados.
         */
        private List<error> checkfields(MedicoDTO medico)
        {
            List<error> formErrors = new List<error>();
            if (medico.especialidades == null || medico.especialidades.Count == 0)
            {
                formErrors.Add(new error()
                {
                    campo = "Especialidades",
                    erro = "Deve ser selecionado ao menos uma especialidade."
                });
            }
            if (String.IsNullOrEmpty(medico.nome))
            {
                formErrors.Add(new error()
                {
                    campo = "Nome",
                    erro = "Nome é obrigatório."
                });
            }
            if (String.IsNullOrEmpty(medico.cpf))
            {
                formErrors.Add(new error()
                {
                    campo = "cpf",
                    erro = "Cpf é obrigatório"
                });
            }
            if (String.IsNullOrEmpty(medico.crm))
            {
                formErrors.Add(new error()
                {
                    campo = "Especialidades",
                    erro = "Crm é obrigatório"
                });
            }
            return formErrors;
        }
    }

    /* Classe para padronizar os erros.
     */
    public class error
    {
        public string campo;
        public string erro;
    }
}