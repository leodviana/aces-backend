function SomenteNumero(e) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
}


function btnConfirmarModalFornecedor(codigo, nome) {
    $('#modalFornecedor').modal('hide');
    $('#CodigoFornecedor').val(codigo);
    $('#NomeFornecedor').val(nome);
}

$(function () {
    $('#CodigoFornecedor').blur(function (e) {
        $('#NomeFornecedor').val("");
        var codigo = $(this).val().trim();
        if (codigo.length > 0) {
            $.ajax({
                type: 'GET',
                url: $(this).data('url'),
                data: { "codigo": codigo },
                success: function(data) {
                    if (!data) {
                        alert("Fornecedor não encontrado.");
                        $(this).focus();
                    } else {
                        $('#NomeFornecedor').val(data);
                        $('#NumeroNotaFiscal').focus();
                    }
                },
                error: function(xhr, status, error) {
                    alert(error);
                }
            });
        }
    });

    $('#tipoConsultaModalFornecedor').change(function (e) {
        var tipo = $(this).val();

        if (tipo == "codigo") {
            $("#descricaoModalFornecedor").attr('onkeypress', 'return SomenteNumero();');
        } else if (tipo == "descricao") {
            $("#descricaoModalFornecedor").removeAttr('onkeypress');
        }
        $('#descricaoModalFornecedor').val("");
        $('#descricaoModalFornecedor').focus();

    });

    // Identificadores de botões que abrem modal: "open-[Nome do Modal]"
    $('#open-Fornecedor').off('click').click(function () {
        // Limpando todas as linhas que estão na tabela.
        // Todos os identificadores de tabelas devem ser únicos, e seguir a nomeclatura "tabela-[Nome do Modal]".
        $('#tabelaFornecedor').empty();

        // Limpando o valor que está no campo descrição.
        $('#descricaoModalFornecedor').val(""),

        // Abrindo o modal.
        // E os identificadores dos modais devem ser "modal-[Nome do Modal]"
            $('#modalFornecedor').modal('show');

        // Requisição ajax passando como paramêtros os campo de tipo de consulta e descrição.
        // Formulários que porventura aparecem dentro de um modal: "form-[Nome do Modal]"
        $('#formFornecedor').off('submit').submit(function (e) {
            e.preventDefault();
            $.ajax({
                type: 'POST',
                url: $(this).data('url'),
                dataType: 'json',
                data: {
                    // Campos que estão dentro de formulários em modais: "[Nome do Campo]-[Nome do Modal]"
                    "tipoConsulta": $('#tipoConsultaModalFornecedor').val(),
                    "descricao": $('#descricaoModalFornecedor').val(),
                },
                success: function (data) {
                    // Limpando todas as linhas que estão na tabela.
                    $('#tabelaFornecedor').empty();
                    // Inserido o Html gerado no controlador no corpo da tabela.
                    $('#tabelaFornecedor').append(data);

                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
            // retornando falso para que o modal não seja finalizado após a requisição.
            return false;
        });
    });

    // Função especifica do Fornecedor.
    /* Se houver mais modais, será necessário criar outras funções semelhantes a esta,
    alterando somente a identificação dos campos.*/
    $('.btnConfirmarModalFornecedor').click(function () {
        alert($(this).data('id'));
        alert($(this).data('nome'));
    });

});