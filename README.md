# Desafio Back-end TDSA Sistemas

## Visão Geral
A api será composta por rotas de consulta, criação e deleção de `medicos`, sendo suas rotas autenticadas e sua autenticação feita através de um token jwt que pode ser recuperado através da rota `/getRandomToken`.

### Versão Atual
Por padrão todas as requisições para `http://servidor:3000/` ou  `https://desafiotdsa-jorge-nascimento.herokuapp.com` recebem a versão mais atual da api.

### Schema 
Todo o acesso a api é através do endereço `http://servidor:3000/` ou `https://desafiotdsa-jorge-nascimento.herokuapp.com`. <break>
  
E todos os dados são transmitidos e recebidos como JSON, onde todos os campos vazios são incluidos como nulos ao invés de omitidos.

## Autenticação
Com excessão da rota usada para realizar o login (No momento é possivel recuperar um token de acesso acessando a rota `http://servidor:3000/getRandomToken`), todas as rotas precisam incluir no cabeçalho da requisição um token de autenticação gerado através do JsonWebToken,
`curl -H "Authorization: Bearer `_`JsonWebToken`_`" http://servidor:3000/`
 Caso contrário a requisição retornara o erro `401 Unauthorized` indicando acesso indevido à api.<break>
 
## Erros no cliente.
Existem possiveis erros ao realizar requisições, podendo elas serem problemas com o tipo do JSON enviado no corpo da requisição, problemas de sintaxe no JSON enviado, etc.<break>
  No caso de erros, será retornado uma mensagem de erro do servidor, com seu status `400 Bad Request`.
> {
  erros: [
      {
          campo: "nome",
          erro: "Nome é obrigatório."
      },
       {
          campo: "cpf",
          erro: "Cpf inválido."
      },
  ]
}


## Parâmetros
Muitos métodos e rotas da api fazem o uso de parâmetros para customizar as requisições, como por exemplo para retornar os dados de um usuário por id, usa-se a rota `http://servidor:3000/Medico/:id` retornando, quando houver registros:<break>

Para as rotas do tipo `POST`, `PUT`, `PATCH` e `DELETE` as requisições serão feitas através do parametro e com os dados no corpo da requisição, sendo elas no formato JSON com `Content-Type` de `application/json`.

## Rotas 

### Autenticação

#### getRandomToken `GET`
Rota do tipo `GET` do endereço `http/servidor:3000/getRandomToken`, que retorna um token deve ser usado para autenticar as outras rotas através do cabeçalho.<break>
  
### Medico

#### getMedicos `GET`
Rota do tipo `GET` do endereço `http/servidor:3000/medico/`, retorna uma lista simplificada com todos os registros dos medicos cadastrados e suas especialidades <break>
Exemplo de retorno:
> [
	 {
			 id: a7ca8325-cca0-4ecb-96ad-46ba85325464,
			 nome: "Gabrielle Martins Souza",
			 cpf: "652.472.120-96",
			 crm: "1010-SC",
			 especialidades: [
					 "Clínico Geral",
					 "Ginicologista"
			 ]
	 },
	 {
			 id: c72e55d9-25ba-42b1-94b8-10d14dfbf7f6,
			 nome: "Luis Souza Oliveira",
			 cpf: "238.677.850-90",
			 crm: "1558-SC",
			 especialidades: [
					 "Ginicologista"
			 ]
	 },
	 {
			 id: 1fb342d1-5118-4e46-acb2-33fcd0689a83,
			 nome: "Leila Barbosa Castro",
			 cpf: "979.006.660-01",
			 crm: "35965-SC",
			 especialidades: [
					 "Cardiologista",
					 "Ginicologista"
			 ]
	 }
]

#### getMedicosByEspecialidade `GET`
Rota do tipo `GET` para o endereço `http://servidor:3000/medico/:query` que recebe o nome da especialidade e retorna todos os médicos que a possuirem.<break>
Exemplo de retorno:
> [
	 {
			 id: a7ca8325-cca0-4ecb-96ad-46ba85325464,
			 nome: "Gabrielle Martins Souza",
			 cpf: "652.472.120-96",
			 crm: "1010-SC",
			 especialidades: [
					 "Clínico Geral",
					 "Ginicologista"
			 ]
	 },
	 {
			 id: c72e55d9-25ba-42b1-94b8-10d14dfbf7f6,
			 nome: "Luis Souza Oliveira",
			 cpf: "238.677.850-90",
			 crm: "1558-SC",
			 especialidades: [
					 "Ginicologista"
			 ]
	 },
	 {
			 id: 1fb342d1-5118-4e46-acb2-33fcd0689a83,
			 nome: "Leila Barbosa Castro",
			 cpf: "979.006.660-01",
			 crm: "35965-SC",
			 especialidades: [
					 "Cardiologista",
					 "Ginicologista"
			 ]
	 }
]


#### createMedico `POST`
Rota do tipo `POST` para o endereço `http://servidor:3000/Medico/` que recebe no corpo da requisição os dados do medicos que se deseja criar o registro.<break>
Exemplo de requisição:
> {
		 "nome": "Gabrielle Martins Souza",
		 "cpf": "652.472.120-96",
		 "crm": "1010-SC",
		 "especialidades": [
				 "Clínico Geral",
				 "Ginicologista"
		 ]
 }



#### deleteMedico `DELETE`
Rota do tipo `DELETE` para o endereço `http://servidor:3000/Medico/:id` que recebe como parametro o id do registro que se deseja remover.<break>
Exemplo de requisição:
> curl -i http://servidor:3000/Medico/{Guid}`
