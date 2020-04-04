function SomenteNumero(e) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
}

//Funcao do V d grid de pesquisa
function btnConfirmarModalEstado(codigo, nome) {
    $('#modalEstado').modal('hide');
    $('#CodigoEstado').val(codigo);
    $('#NomeEstado').val(nome);
}

//Funcao "OnExit" do codigo
$(function () {
    $('#CodigoEstado').blur(function (e) {
        $('#NomeEstado').val("");
        var codigo = $(this).val().trim();
        if (codigo.length > 0) {
            $.ajax({
                type: 'GET',
                url: $(this).data('url'),
                data: { "codigo": codigo },
                success: function(data) {
                    if (!data) {
                        alert("Estado Civil não encontrado.");
                        $(this).focus();
                    } else {
                        $('#NomeEstado').val(data);
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
    $('#tipoConsultaModalEstado').change(function (e) {
        var tipo = $(this).val();
        if (tipo == "codigo") {
            $("#descricaoModalEstado").attr('onkeypress', 'return SomenteNumero();');
           
        } else if (tipo == "descricao") {
            $("#descricaoModalEstado").removeAttr('onkeypress');
           
        }
        $('#descricaoModalEstado').val("");
        $('#descricaoModalEstado').focus();

    });

    //Funcao de do botao pesquisar do modal
    // Identificadores de botões que abrem modal: "open-[Nome do Modal]"
    $('#open-Estado').off('click').click(function () {

        if (!$('#modalEstado').data('carregadefault')) {
            // Limpando todas as linhas que estão na tabela.
            // Todos os identificadores de tabelas devem ser únicos, e seguir a nomeclatura "tabela-[Nome do Modal]".
            $('#tabelaEstado').empty();
            

        }
        else //preenchendo previamento a os dados
        {
            
            $.ajax({
                type: 'POST',
                url: $("#formEstado").data('url'),
                dataType: 'json',
                data: {
                    "tipoConsulta": "todos",
                    "filtro": "",
                },
                success: function (data) {
                    $('#tabelaEstado').empty();
                    $('#tabelaEstado').append(data);
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }


        // Limpando o valor que está no campo descrição.
        $('#descricaoModalEstado').val(""),

        // Abrindo o modal.
        // E os identificadores dos modais devem ser "modal-[Nome do Modal]"
        $('#modalEstado').modal('show');

        // Requisição ajax passando como paramêtros os campo de tipo de consulta e descrição.
        // Formulários que porventura aparecem dentro de um modal: "form-[Nome do Modal]"
        $('#formEstado').off('submit').submit(function (e) {
            e.preventDefault();
            $.ajax({
                type: 'POST',
                url: $(this).data('url'),
                dataType: 'json',
                data: {
                    // Campos que estão dentro de formulários em modais: "[Nome do Campo]-[Nome do Modal]"
                    "tipoConsulta": $('#tipoConsultaModalEstado').val(),
                    "filtro": $('#descricaoModalEstado').val(),
                },
                success: function (data) {
                    // Limpando todas as linhas que estão na tabela.
                    $('#tabelaEstado').empty();
                    // Inserido o Html gerado no controlador no corpo da tabela.
                    $('#tabelaEstado').append(data);

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