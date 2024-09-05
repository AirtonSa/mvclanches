namespace LanchesMac.Services
{
    public interface ISeedUserRoleInitial
    {
        void SeedRoles();  //vai ser incrementada a criação dos perfis(id,nome,normalizeName(mesmo nome só em caixa alta)
        void SeedUsers(); //Para criar os usuários e atribuir os usuarios aos perfis(user id,role id)
    }
}
