CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE "Especialidades" (
    id uuid NOT NULL,
    nome text NOT NULL,
    CONSTRAINT "PK_Especialidades" PRIMARY KEY (id)
);

CREATE TABLE "Medicos" (
    id uuid NOT NULL,
    nome character varying(100) NOT NULL,
    cpf text NOT NULL,
    crm text NOT NULL,
    CONSTRAINT "PK_Medicos" PRIMARY KEY (id)
);

CREATE TABLE "MedicosEspecialidades" (
    id uuid NOT NULL,
    especialidadeid uuid NULL,
    medicoid uuid NULL,
    CONSTRAINT "PK_MedicosEspecialidades" PRIMARY KEY (id),
    CONSTRAINT "FK_MedicosEspecialidades_Especialidades_especialidadeid" FOREIGN KEY (especialidadeid) REFERENCES "Especialidades" (id) ON DELETE RESTRICT,
    CONSTRAINT "FK_MedicosEspecialidades_Medicos_medicoid" FOREIGN KEY (medicoid) REFERENCES "Medicos" (id) ON DELETE RESTRICT
);

CREATE INDEX "IX_MedicosEspecialidades_especialidadeid" ON "MedicosEspecialidades" (especialidadeid);

CREATE INDEX "IX_MedicosEspecialidades_medicoid" ON "MedicosEspecialidades" (medicoid);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20200818011108_create-initial', '3.1.7');

