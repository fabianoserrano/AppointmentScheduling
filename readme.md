# Sistema de Agendamento de Consultas Médicas - Health&Med

É uma aplicação .NET que simula um Sistema de Agendamento de Consultas Médicas, permitindo que os pacientes possam verificar os médicos disponíveis e agendar uma consulta em um horário disponível no sistema. A solução foi desenvolvida utilizando a linguagem de programação C# na versão 6.0 do .NET e banco de dados SQL Server com publicação dentro de um cluster Kubernets e testes sendo executados via pipeline no GitHub Actions.

## Requisitos Funcionais
Disponibilizar área logada utilizando e-mail e senha de usuário(médico/paciente) utilizando JWT.

Disponibilizar endpoint para inclusão de um novo usuário/paciente para uso da aplicação.

Disponibilizar endpoint para inclusão de um novo Usuário/médico para uso da aplicação.

Disponibilizar endpoint para buscar por médicos pelo paciente.

Disponibilizar enpoint para inclusão/edição de horários disponíveis pelo médico.

Disponibilizar endpoint para agendamento de consultas pelo paciente.

Notificar o médico por e-mail após realização de agendamento pelo paciente.

### Recursos:
* Gerenciar usuários (Médicos/Pacientes)
* Gerenciar agendamentos (Médico)
* Gerenciar horários (Médico)
* Permitir o paciente realizar agendamento com um médico (Paciente)

### Regras de negócio:
* Para efetuar login no sistema, o usuário deverá se cadastrar e possuir e-mail e senha para autenticação na aplicação
* O paciente, ao efetuar login, poderá buscar por médicos e agendar uma consulta em um horário disponibilizado pelo médico
* O médico deverá informar uma data e horário de início e data e horário fim válidos para disponibilidade de agendamentos
* O médico poderá cadastrar e editar horários para disponibilizar nos agendamentos pelos pacientes

## Critérios de aceite:

Para cadastro de um novo **usuário** na plataforma, deverá ser informado o nome, e-mail e uma senha:

**POST /Patient**
```
{
  "name": "João da Silva",
  "cpf": "123.456.789-01",
  "email": "joao.silva@email.com",
  "password": "******"
}

```

Para efetuar **login**, deverá ser informado o e-mail cadastrado:

**POST / Login**

```
{
  "email": "joao.silva@email.com",
  "password": "********"
}
```

Após isso, caso o e-mail esteja cadastrado no banco de dados do sistema e a senha seja válida, o sistema retornará o *token* de acesso que possui validade de 8 horas, conforme exemplo abaixo, com informações do usuário logado:

````
{
  "authenticated": true,
  "created": "2024-07-21 16:29:05",
  "expirationDate": "2024-07-22 00:29:05",
  "acessToken": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6WyJqb2FvLnNpbHZhQGVtYWlsLmNvbSIsImpvYW8uc2lsdmFAZW1haWwuY29tIl0sImp0aSI6ImM5Njg1MWNhLTVmNzUtNDk0YS05ZTlhLWI5NWNhMmUwOTJlZSIsIm5iZiI6MTcyMTU5MDE0NSwiZXhwIjoxNzIxNjE4OTQ1LCJpYXQiOjE3MjE1OTAxNDUsImlzcyI6IkV4ZW1wbG9Jc3N1ZXIiLCJhdWQiOiJFeGVtcGxvQXVkaWVuY2UifQ.cwQz5878YWdfZfSxEIJnEsvHxD__TX0HbWyWSepqBQDvG9fdTc54-fWICZLi40Msra-xWYjbRDmiSbvjVd_Jmd2Ow-bifaYXZEPQSbpK7jLfVP1Nhccgt6GlQLWtT4h6BsEQR61j70pLNU1L81CP-zJRx6irCM82O_zbD-R2e9iucKOVVuRh_tFOOgReX1eIbxkJVUMOsAVpXX214utC8wqQhnyCxoY12cM1V9QMux2UYj2B8imVo0NAOC7n50FW8BZ8urOEgugX45y8ER0i4biZTUW6qCwe0T-QGA6pkFMvbfY2FWVEmQgrUBheTc6kKQxpbrCq5HAngM6kowD3vg",
  "userName": "joao.silva@email.com",
  "name": "João Silva",
  "message": "Usuário Logado com sucesso"
}
````

Deverá ser copiado o *acessToken* e clicar no botão **Authorize** passando as informações necessárias conforme exemplo abaixo:

````
Bearer (apiKey)
Entre com o Token JWT
Name: Authorization
In: header
Value: Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6WyJqb2FvLnNpbHZhQGVtYWlsLmNvbSIsImpvYW8uc2lsdmFAZW1haWwuY29tIl0sImp0aSI6ImM5Njg1MWNhLTVmNzUtNDk0YS05ZTlhLWI5NWNhMmUwOTJlZSIsIm5iZiI6MTcyMTU5MDE0NSwiZXhwIjoxNzIxNjE4OTQ1LCJpYXQiOjE3MjE1OTAxNDUsImlzcyI6IkV4ZW1wbG9Jc3N1ZXIiLCJhdWQiOiJFeGVtcGxvQXVkaWVuY2UifQ.cwQz5878YWdfZfSxEIJnEsvHxD__TX0HbWyWSepqBQDvG9fdTc54-fWICZLi40Msra-xWYjbRDmiSbvjVd_Jmd2Ow-bifaYXZEPQSbpK7jLfVP1Nhccgt6GlQLWtT4h6BsEQR61j70pLNU1L81CP-zJRx6irCM82O_zbD-R2e9iucKOVVuRh_tFOOgReX1eIbxkJVUMOsAVpXX214utC8wqQhnyCxoY12cM1V9QMux2UYj2B8imVo0NAOC7n50FW8BZ8urOEgugX45y8ER0i4biZTUW6qCwe0T-QGA6pkFMvbfY2FWVEmQgrUBheTc6kKQxpbrCq5HAngM6kowD3vg
````

Após isso, os endpoints da aplicação estarão disponíveis para uso, de acordo com o perfil do usuário, sendo este, médico ou paciente.

Para cadastro de um **médico**, deverá ser informado o nome, cpf, e-mail, senha e CRM, conforme exemplo abaixo:

**POST /Doctor**

````
{
  "name": "Dr. Miguel Souza",
  "cpf": "123.456.789-11",
  "email": "drmiguelsouza@healthmed.com",
  "password": "********",
  "crm": "1234"
}
````

Para criação de um novo **horário** disponível, deverá ser informado o id do médico, data e horário, conforme exemplo abaixo:

**POST /Appointment**

````
{
  "doctorId": 1,
  "date": "2024-08-02T08:00:00.037Z"
}
````

Para alteração de um **horário** disponibilizado, deverá ser informado Id do agendamento, o Id do médico, data e horário, conforme exemplo abaixo:

**PUT /Availability**

````
{
  "id": 1,
  "doctorId": 2,
  "date": "2024-08-02T11:00:00.731Z"
}
````

Para verificar **horários disponíveis** pelo médico de escolha, preencher o Id do Médico conforme exemplo abaixo:

**GET /Appointment/GetAvailableAppointments/{doctorId}**
````
{
  "doctorId": 1,
}
````
Para **agendar** uma consulta, informar o id do horário e o id do paciente conforme exemplo abaixo:

**PUT /Appointment/ScheduleAppointment**

````
{
  "id": 1,
  "patientId": 1
}
````


## Execução:
Abra a solução (AppointmentScheduling.sln), preferencialmente, na versão 2022 ou posterior do Microsoft Visual Studio.

Ter instalado o SQL Server Management Studio, de preferência a versão 19 ou posterior.

Altere a string de conexão (ConnectionString) da base de dados:

````
Path: AppointmentSchedulingService\Consumers\API\appsettings.json

"ConnectionStrings": {
    "DevConnectionString": "Data Source=(localdb)\\MSSQLLocalDB;Database=DbMedAppintments;Integrated Security=true"
},
````

## Banco de dados:
Utilizar o entity framework (pasta Migrations no projeto BookingService):
````
AppointmentSchedulingService\Adpters\Data
````
Abra o console do gerenciador de pacotes e gere a Migration:
````
Add-Migration InitialMigration -Project Data
Update-Database
````


## Diagramas
Para melhor visualização e navegação nos diagramas, realizar login no site:
* [https://app.diagrams.net/](https://app.diagrams.net/)

Link dos diagramas e apresentação do projeto:
* [medappointments.drawio](https://drive.google.com/file/d/1-KnEti7zq4l8iJwbpSu9gnDOIUGbByg8/view?usp=sharing)
