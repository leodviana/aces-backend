function SomenteNumero(e) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
}

//Funcao do V d grid de pesquisa
function btnConfirmarModalSexo(codigo, nome) {
    $('#modalSexo').modal('hide');
    $('#CodigoSexo').val(codigo);
    $('#NomeSexo').val(nome);
}

//Funcao "OnExit" do codigo
$(function () {
    $('#CodigoSexo').blur(function (e) {
        $('#NomeSexo').val("");
        var codigo = $(this).val().trim();
        if (codigo.length > 0) {
            $.ajax({
                type: 'GET',
                url: $(this).data('url'),
                data: { "codigo": codigo },
                success: function(data) {
                    if (!data) {
                        alert("Sexo não encontrado.");
                        $(this).focus();
                    } else {
                        $('#NomeSexo').val(data);
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
    $('#tipoConsultaModalSexo').change(function (e) {
        var tipo = $(this).val();

        if (tipo == "codigo") {
            $("#descricaoModalSexo").attr('onkeypress', 'return SomenteNumero();');
           
        } else if (tipo == "descricao") {
            $("#descricaoModalSexo").removeAttr('onkeypress');
           
        }
        $('#descricaoModalSexo').val("");
        $('#descricaoModalSexo').focus();

    });

    //Funcao de do botao pesquisar do modal
    // Identificadores de botões que abrem modal: "open-[Nome do Modal]"
    $('#open-Sexo').off('click').click(function () {
        // Limpando todas as linhas que estão na tabela.
        // Todos os identificadores de tabelas devem ser únicos, e seguir a nomeclatura "tabela-[Nome do Modal]".
        if (!$('#modalSexo').data('carregadefault')) {
            // Limpando todas as linhas que estão na tabela.
            // Todos os identificadores de tabelas devem ser únicos, e seguir a nomeclatura "tabela-[Nome do Modal]".
            $('#tabelaSexo').empty();


        }
        else //preenchendo previamento a os dados
        {

            $.ajax({
                type: 'POST',
                url: $("#formSexo").data('url'),
                dataType: 'json',
                data: {
                    "tipoConsulta": "todos",
                    "filtro": "",
                },
                success: function (data) {
                    $('#tabelaSexo').empty();
                    $('#tabelaSexo').append(data);
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }
        // Limpando o valor que está no campo descrição.
        $('#descricaoModalSexo').val(""),

        // Abrindo o modal.
        // E os identificadores dos modais devem ser "modal-[Nome do Modal]"
            $('#modalSexo').modal('show');

        // Requisição ajax passando como paramêtros os campo de tipo de consulta e descrição.
        // Formulários que porventura aparecem dentro de um modal: "form-[Nome do Modal]"
        $('#formSexo').off('submit').submit(function (e) {
            e.preventDefault();
            $.ajax({
                type: 'POST',
                url: $(this).data('url'),
                dataType: 'json',
                data: {
                    // Campos que estão dentro de formulários em modais: "[Nome do Campo]-[Nome do Modal]"
                    "tipoConsulta": $('#tipoConsultaModalSexo').val(),
                    "filtro": $('#descricaoModalSexo').val(),
                },
                success: function (data) {
                    // Limpando todas as linhas que estão na tabela.
                    $('#tabelaSexo').empty();
                    // Inserido o Html gerado no controlador no corpo da tabela.
                    $('#tabelaSexo').append(data);

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