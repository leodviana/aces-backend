function SomenteNumero(e) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
}

//Funcao do V d grid de pesquisa
function btnConfirmarModalCliente(codigo, nome) {
    
    $('#modalCliente').modal('hide');
    $('#CodigoCliente').val(codigo);
    $('#NomeCliente').val(nome);
}

//Funcao "OnExit" do codigo
$(function () {
    $('#CodigoCliente').blur(function (e) {
        $('#NomeCliente').val("");
        
        var codigo = $(this).val().trim();
        
        if (codigo.length > 0) {

            $.ajax({
                type: 'GET',
                url: $(this).data('url'),

                data: { "codigo": codigo },
                success: function (data) {
                    
                    if (!data) {
                        alert("Cliente não encontrado.");
                        $(this).focus();
                    } else {
                        $('#NomeCliente').val(data);
                        $('#NomeCliente').focus();
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }
    });

    //Funcao change do dropdowntipo do modal de consulta
    $('#tipoConsultaModalCliente').change(function (e) {
        var tipo = $(this).val();
        if (tipo == "codigo") {
            $("#descricaoModalCliente").attr('onkeypress', 'return SomenteNumero();');

        } else if (tipo == "descricao") {
            $("#descricaoModalCliente").removeAttr('onkeypress');

        }
        $('#descricaoModalCliente').val("");
        $('#descricaoModalCliente').focus();

    });

    //Funcao de do botao pesquisar do modal
    // Identificadores de botões que abrem modal: "open-[Nome do Modal]"
    $('#open-Cliente').off('click').click(function () {

        if (!$('#modalCliente').data('carregadefault')) {
            // Limpando todas as linhas que estão na tabela.
            // Todos os identificadores de tabelas devem ser únicos, e seguir a nomeclatura "tabela-[Nome do Modal]".
            $('#tabelaCliente').empty();


        }
        else //preenchendo previamento a os dados
        {

            $.ajax({
                type: 'POST',
                url: $("#formCliente").data('url'),
                dataType: 'json',
                data: {
                    "tipoConsulta": "todos",
                    "filtro": "",
                },
                success: function (data) {
                    $('#tabelaCliente').empty();
                    $('#tabelaCliente').append(data);
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }


        // Limpando o valor que está no campo descrição.
        $('#descricaoModalCliente').val(""),

        // Abrindo o modal.
        // E os identificadores dos modais devem ser "modal-[Nome do Modal]"
        $('#modalCliente').modal('show');

        // Requisição ajax passando como paramêtros os campo de tipo de consulta e descrição.
        // Formulários que porventura aparecem dentro de um modal: "form-[Nome do Modal]"
        $('#formCliente').off('submit').submit(function (e) {
            e.preventDefault();
            $.ajax({
                type: 'POST',
                url: $(this).data('url'),
                dataType: 'json',
                data: {
                    // Campos que estão dentro de formulários em modais: "[Nome do Campo]-[Nome do Modal]"
                    "tipoConsulta": $('#tipoConsultaModalCliente').val(),
                    "filtro": $('#descricaoModalCliente').val(),
                },
                success: function (data) {
                    // Limpando todas as linhas que estão na tabela.
                    $('#tabelaCliente').empty();
                    // Inserido o Html gerado no controlador no corpo da tabela.
                    $('#tabelaCliente').append(data);

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