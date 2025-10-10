namespace Core.Entities.Identity
{
    public class CurrentRole
    {

        //---------------------------------------------------------------------------------------------------


        public string Email { get; set; }

        public int MercadoId { get; set; }

        public int EmpresaId { get; set; }

        public int GrupolojaId { get; set; }

        public int LojaId { get; set; }

        public bool IsSuperAdmin { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsRevisor { get; set; }

        public bool IsSupervisor { get; set; }

        public bool IsGerenteLoja { get; set; }

        public bool IsColaborador { get; set; }

        public bool IsBasic { get; set; }

        public string RoleName { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}