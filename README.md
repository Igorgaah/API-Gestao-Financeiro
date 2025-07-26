# 💸 Financeiro API

API RESTful para gestão de gastos pessoais, desenvolvida com C# e ASP.NET Core, utilizando Entity Framework Core e autenticação JWT. Permite o controle de gastos com filtros por data e categoria, exportação de dados, autenticação com refresh tokens e controle de usuários e permissões (roles).

---

## 🚀 Funcionalidades

* ✅ CRUD completo de gastos (`Gasto`)
* 🔍 Filtro por data e categoria
* 🔐 Autenticação e autorização com **JWT + Refresh Token**
* 👤 Cadastro de usuários e validação via banco de dados
* 🛡️ Controle de roles: `admin`, `user`
* 📁 Exportação de gastos (ex: JSON/CSV - em construção)
* 🌐 Swagger UI integrado

---

## 🧱 Tecnologias Utilizadas

* [.NET 7 ou superior](https://dotnet.microsoft.com/)
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server (via LocalDB ou outro)
* JWT Authentication (com Refresh Token)
* Swagger / Swashbuckle

---

## 📦 Instalação

### 1. Clone o projeto

```
git clone https://github.com/seu-usuario/GastosAPI.git
cd GastosAPI
```

### 2. Configure o banco de dados

No `appsettings.json`, configure a connection string:

```
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GastosDb;Trusted_Connection=True;"
}
```

> Ou substitua conforme seu ambiente (SQL Server, PostgreSQL etc.)

### 3. Execute as migrações

```
dotnet ef database update
```

### 4. Rode a aplicação

```
dotnet run
```

Acesse o Swagger: `https://localhost:5001/swagger`

---

## 🔑 Autenticação JWT

* Faça login em `/api/auth/login` com usuário válido
* O token JWT será retornado
* Utilize o token nos endpoints protegidos via `Authorization: Bearer <token>`

---

## 📌 Endpoints Principais

| Verbo  | Rota                 | Descrição             |
| ------ | -------------------- | --------------------- |
| GET    | `/api/gastos`        | Lista todos os gastos |
| GET    | `/api/gastos/{id}`   | Detalha um gasto      |
| POST   | `/api/gastos`        | Cria um novo gasto    |
| PUT    | `/api/gastos/{id}`   | Atualiza um gasto     |
| DELETE | `/api/gastos/{id}`   | Remove um gasto       |
| POST   | `/api/auth/login`    | Autentica usuário     |
| POST   | `/api/auth/register` | Registra novo usuário |

---

## 👥 Roles e Permissões

* `admin`: acesso total
* `user`: acesso aos próprios gastos
* (validação e controle feitos via JWT Claims e banco de dados)

---

## 🛠 Estrutura do Projeto

```
GastosAPI/
│
├── Controllers/
│   ├── GastosController.cs
│   └── AuthController.cs
│
├── Models/
│   ├── Gasto.cs
│   └── Usuario.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Services/
│   └── TokenService.cs
│
├── appsettings.json
└── Program.cs
```

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

---

## 🙋‍♂️ Autor

Desenvolvido por [Igor Rafael Marcelo Morais](https://github.com/Igorgaah)

