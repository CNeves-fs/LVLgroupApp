using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Entities.Ocorrencias;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Ocorrencias.Commands.Update
{
    public class UpdateOcorrenciaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string CodeId { get; set; }  // Format : YYYYMMDD-OCORR-LLLL-XXXXX

        public DateTime DataOcorrencia { get; set; }

        public string EmailAutor { get; set; }

        public DateTime DataEntradaSistemaOcorrencia { get; set; }





        public int CategoriaId { get; set; }

        public int InterOcorrenciaId { get; set; }          // interlojas

        public bool MasterOcorrencia { get; set; }          // interlojas

        public int TipoOcorrenciaId { get; set; }

        public int StatusId { get; set; }






        public string OcorrenciaNome { get; set; }

        public string Descrição { get; set; }

        public string Comentário { get; set; }

        public int TotalFicheiros { get; set; }

        public string OcorrenciaFolder { get; set; }





        public int EmpresaId { get; set; }

        public int GrupolojaId { get; set; }

        public int LojaId { get; set; }

        public int MercadoId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateOcorrenciaCommandHandler : IRequestHandler<UpdateOcorrenciaCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IOcorrenciaRepository _ocorrenciaRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateOcorrenciaCommandHandler(IOcorrenciaRepository ocorrenciaRepository, IUnitOfWork unitOfWork)
            {
                _ocorrenciaRepository = ocorrenciaRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateOcorrenciaCommand command, CancellationToken cancellationToken)
            {
                var ocorrencia = await _ocorrenciaRepository.GetByIdAsync(command.Id);

                if (ocorrencia == null)
                {
                    return Result<int>.Fail($"Ocorrencia Not Found.");
                }
                else
                {
                    ocorrencia.CodeId = string.IsNullOrEmpty(command.CodeId) ? ocorrencia.CodeId : command.CodeId;
                    DateTime dt = DateTime.ParseExact("01/01/2000", "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    ocorrencia.DataOcorrencia = (command.DataOcorrencia.CompareTo(dt) < 0) ? ocorrencia.DataOcorrencia : command.DataOcorrencia;
                    ocorrencia.DataEntradaSistemaOcorrencia = (command.DataEntradaSistemaOcorrencia.CompareTo(dt) < 0) ? ocorrencia.DataEntradaSistemaOcorrencia : command.DataEntradaSistemaOcorrencia;
                    ocorrencia.EmailAutor = string.IsNullOrEmpty(command.EmailAutor) ? ocorrencia.EmailAutor : command.EmailAutor;


                    ocorrencia.CategoriaId = (command.CategoriaId == 0) ? ocorrencia.CategoriaId : command.CategoriaId;
                    ocorrencia.TipoOcorrenciaId = (command.TipoOcorrenciaId == 0) ? ocorrencia.TipoOcorrenciaId : command.TipoOcorrenciaId;
                    ocorrencia.StatusId = (command.StatusId == 0) ? ocorrencia.StatusId : command.StatusId;
                    ocorrencia.InterOcorrenciaId = (command.InterOcorrenciaId == 0) ? ocorrencia.InterOcorrenciaId : command.InterOcorrenciaId;
                    ocorrencia.MasterOcorrencia = (command.MasterOcorrencia == false) ? ocorrencia.MasterOcorrencia : command.MasterOcorrencia;



                    ocorrencia.OcorrenciaNome = string.IsNullOrEmpty(command.OcorrenciaNome) ? ocorrencia.OcorrenciaNome : command.OcorrenciaNome;
                    ocorrencia.Descrição = string.IsNullOrEmpty(command.Descrição) ? ocorrencia.Descrição : command.Descrição;
                    ocorrencia.Comentário = string.IsNullOrEmpty(command.Comentário) ? ocorrencia.Comentário : command.Comentário;
                    ocorrencia.TotalFicheiros = (command.TotalFicheiros == 0) ? ocorrencia.TotalFicheiros : command.TotalFicheiros;            
                    ocorrencia.OcorrenciaFolder = string.IsNullOrEmpty(command.OcorrenciaFolder) ? ocorrencia.OcorrenciaFolder : command.OcorrenciaFolder;



                    ocorrencia.EmpresaId = (command.EmpresaId == 0) ? ocorrencia.EmpresaId : command.EmpresaId;
                    ocorrencia.GrupolojaId = (command.GrupolojaId == 0) ? ocorrencia.GrupolojaId : command.GrupolojaId;
                    ocorrencia.LojaId = (command.LojaId == 0) ? ocorrencia.LojaId : command.LojaId;
                    ocorrencia.MercadoId = (command.MercadoId == 0) ? ocorrencia.MercadoId : command.MercadoId;
                    


                    await _ocorrenciaRepository.UpdateAsync(ocorrencia);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(ocorrencia.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}