using System.Collections.Generic;

namespace Core.Constants
{
    public static class StatusDisplay
    {

        //---------------------------------------------------------------------------------------------------


        //claim na loja a aguardar mais fotos/informação
        public static class StatusPendenteEmLoja
        {
            public const string PL_Fotos = "PENDENTE EM LOJA: AGUARDA MAIS FOTOS";
            public const string PL_Informa = "PENDENTE EM LOJA: AGUARDA MAIS INFORMAÇÕES";
            public const string PL_Infiltra = "PENDENTE EM LOJA: AGUARDA TESTE INFILTRAÇÃO";
            public const string PL_Camurça = "PENDENTE EM LOJA: AGUARDA TESTE CAMURÇA";
            public const string PL_Algodão = "PENDENTE EM LOJA: AGUARDA TESTE ALGODÃO";
            public const string PL_corfundo = "#1e92e9"; // azul vivo
            public const string PL_cortexto = "#f3f3f3"; // cinza muito claro
        }


        //---------------------------------------------------------------------------------------------------


        //claim na sede a aguardar validação pelo revisor
        public static class StatusAguardaValidação
        {
            public const string Avalidação = "AGUARDA VALIDAÇÃO";
            public const string AV_corfundo = "#f4eb7c"; // amarelo claro
            public const string AV_cortexto = "#333333"; // cinza muito escuro
        }


        //---------------------------------------------------------------------------------------------------


        //claim na sede a aguardar decisão
        public static class StatusAguardaDecisão
        {
            public const string ADecisão = "AGUARDA DECISÃO";
            public const string AD_corfundo = "#f1df05"; // amarelo
            public const string AD_cortexto = "#333333"; // cinza muito escuro
        }


        //---------------------------------------------------------------------------------------------------


        //claim na sede a aguardar opinião
        public static class StatusAguardaOpinião
        {
            public const string AO_GerenteLoja = "AGUARDA OPINIÃO GERENTE DE LOJA";
            public const string AO_Supervisor = "AGUARDA OPINIÃO SUPERVISOR";
            public const string AO_Revisor = "AGUARDA OPINIÃO REVISOR";
            public const string AO_Fornecedor = "AGUARDA OPINIÃO FORNECEDOR";
            public const string AO_corfundo = "#c27ba0"; // roxo
            public const string AO_cortexto = "#f3f3f3"; // cinza muito claro
        }


        //---------------------------------------------------------------------------------------------------


        //claim na loja a aguardar resolução com o cliente final (claim aceite)
        public static class StatusAceiteAguardaResoluçãoCliente
        {
            public const string Aceite = "RECLAMAÇÃO ACEITE: AGUARDA RESOLUÇÃO COM O CLIENTE";
            public const string AA_corfundo = "#348c0e"; // verde escuro
            public const string AA_cortexto = "#f3f3f3"; // cinza muito claro
        }


        //---------------------------------------------------------------------------------------------------


        //claim na loja a aguardar resolução com o cliente final (claim não aceite)
        public static class StatusRecusadaAguardaResoluçãoCliente
        {
            public const string NãoAceite = "RECLAMAÇÃO NÃO ACEITE: AGUARDA RESOLUÇÃO COM O CLIENTE";
            public const string NA_corfundo = "#fd1313"; // vermelho
            public const string NA_cortexto = "#f3f3f3"; // cinza muito claro
        }


        //---------------------------------------------------------------------------------------------------


        //fim da claim (fechada na loja)
        public static class StatusFecharEmLoja
        {
            public const string FL_Nãoaceite = "FECHADA EM LOJA POR RECLAMAÇÃO NÃO ACEITE";
            public const string FL_Trocadireta = "FECHADA EM LOJA POR TROCA DIRETA";
            public const string FL_Reparação = "FECHADA EM LOJA POR REPARAÇÃO DE ARTIGO";
            public const string FL_Trocaartigo = "FECHADA EM LOJA POR TROCA DE ARTIGO";
            public const string FL_Devdinheiro = "FECHADA EM LOJA POR DEVOLUÇÃO DE DINHEIRO";
            public const string FL_Notacrédito = "FECHADA EM LOJA POR NOTA DE CRÉDITO";
            public const string FL_corfundo = "#567765"; // cinza
            public const string FL_cortexto = "#f3f3f3"; // cinza muito claro
        }


        //---------------------------------------------------------------------------------------------------

    }
}