# MeuProjeto

Projeto de estudo com arquitetura moderna em .NET 9 explorando práticas profissionais como autenticação com JWT, microserviços, mensageria, observabilidade, Docker e Kubernetes.

---

## Estrutura de projetos

```bash
src/
├── Api/             # API ASP.NET Core (endpoints REST, controllers)
├── Application/     # Casos de uso, serviços de aplicação
├── Core/            # Entidades, interfaces, objetos de domínio
├── Infrastructure/  # Implementações (EF Core, serviços externos)
tests/
└── Api.Tests/       # Testes unitários e de integração
