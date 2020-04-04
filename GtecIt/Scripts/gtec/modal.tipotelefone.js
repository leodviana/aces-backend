function SomenteNumero(e) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
}

//Funcao do V d grid de pesquisa
function btnConfirmarModalTipoTelefone(codigo, nome) {
    $('#modalTipoTelefone').modal('hide');
    $('#CodigoTipoTelefone').val(codigo);
    $('#NomeTipoTelefone').val(nome);
}

//Funcao "OnExit" do codigo
$(function () {
    $('#CodigoTipoTelefone').blur(function (e) {
        $('#NomeTipoTelefone').val("");
        var codigo = $(this).val().trim();
        if (codigo.length > 0) {
            $.ajax({
                type: 'GET',
                url: $(this).data('url'),
                data: { "codigo": codigo },
                success: function(data) {
                    if (!data) {
                        alert("Tipo Telefone não encontrado.");
                        $(this).focus();
                    } else {
                        $('#NomeTipoTelefone').val(data);
                        $('#NumeroNotaFiscal').focus();
                    }
                },
                error: function(xhr, status, error) {
                    alert(error);
                }
            });
        }
    });

    //Funcao change do dropdowntipo do modal de consulta
    $('#tipoConsultaModalTipoTelefone').change(function (e) {
        var tipo = $(this).val();
        if (tipo == "codigo") {
            $("#descricaoModalTipoTelefone").attr('onkeypress', 'return SomenteNumero();');
           
        } else if (tipo == "descricao") {
            $("#descricaoModalTipoTelefone").removeAttr('onkeypress');
           
        }
        $('#descricaoModalTipoTelefone').val("");
        $('#descricaoModalTipoTelefone').focus();

    });

    //Funcao de do botao pesquisar do modal
    // Identificadores de botões que abrem modal: "open-[Nome do Modal]"
    $('#open-TipoTelefone').off('click').click(function () {

        if (!$('#modalTipoTelefone').data('carregadefault')) {
            // Limpando todas as linhas que estão na tabela.
            // Todos os identificadores de tabelas devem ser únicos, e seguir a nomeclatura "tabela-[Nome do Modal]".
            $('#tabelaTipoTelefone').empty();
            

        }
        else //preenchendo previamento a os dados
        {
            
            $.ajax({
                type: 'POST',
                url: $("#formTipoTelefone").data('url'),
                dataType: 'json',
                data: {
                    "tipoConsulta": "todos",
                    "filtro": "",
                },
                success: function (data) {
                    $('#tabelaTipoTelefone').empty();
                    $('#tabelaTipoTelefone').append(data);
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }


        // Limpando o valor que está no campo descrição.
        $('#descricaoModalTipoTelefone').val(""),

        // Abrindo o modal.
        // E os identificadores dos modais devem ser "modal-[Nome do Modal]"
        $('#modalTipoTelefone').modal('show');

        // Requisição ajax passando como paramêtros os campo de tipo de consulta e descrição.
        // Formulários que porventura aparecem dentro de um modal: "form-[Nome do Modal]"
        $('#formTipoTelefone').off('submit').submit(function (e) {
            e.preventDefault();
            $.ajax({
                type: 'POST',
                url: $(this).data('url'),
                dataType: 'json',
                data: {
                    // Campos que estão dentro de formulários em modais: "[Nome do Campo]-[Nome do Modal]"
                    "tipoConsulta": $('#tipoConsultaModalTipoTelefone').val(),
                    "filtro": $('#descricaoModalTipoTelefone').val(),
                },
                success: function (data) {
                    // Limpando todas as linhas que estão na tabela.
                    $('#tabelaTipoTelefone').empty();
                    // Inserido o Html gerado no controlador no corpo da tabela.
                    $('#tabelaTipoTelefone').append(data);

                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
            // retornando falso para que o modal não seja finalizado após a requisição.
            return false;
        });
    });

});
   

    


