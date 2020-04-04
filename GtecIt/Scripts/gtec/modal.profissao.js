function SomenteNumero(e) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
}

//Funcao do V d grid de pesquisa
function btnConfirmarModalProfissao(codigo, nome) {
    $('#modalProfissao').modal('hide');
    $('#CodigoProfissao').val(codigo);
    $('#NomeProfissao').val(nome);
}

//Funcao "OnExit" do codigo
$(function () {
    $('#CodigoProfissao').blur(function (e) {
        $('#NomeProfissao').val("");
        var codigo = $(this).val().trim();
        if (codigo.length > 0) {
            $.ajax({
                type: 'GET',
                url: $(this).data('url'),
                data: { "codigo": codigo },
                success: function(data) {
                    if (!data) {
                        alert("Profissao não encontrada.");
                        $(this).focus();
                    } else {
                        $('#NomeProfissao').val(data);
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
    $('#tipoConsultaModalProfissao').change(function (e) {
        var tipo = $(this).val();
        if (tipo == "codigo") {
            $("#descricaoModalProfissao").attr('onkeypress', 'return SomenteNumero();');
           
        } else if (tipo == "descricao") {
            $("#descricaoModalProfissao").removeAttr('onkeypress');
           
        }
        $('#descricaoModalProfissao').val("");
        $('#descricaoModalProfissao').focus();

    });

    //Funcao de do botao pesquisar do modal
    // Identificadores de botões que abrem modal: "open-[Nome do Modal]"
    $('#open-Profissao').off('click').click(function () {

        if (!$('#modalProfissao').data('carregadefault')) {
            // Limpando todas as linhas que estão na tabela.
            // Todos os identificadores de tabelas devem ser únicos, e seguir a nomeclatura "tabela-[Nome do Modal]".
            $('#tabelaProfissao').empty();
            

        }
        else //preenchendo previamento a os dados
        {
            
            $.ajax({
                type: 'POST',
                url: $("#formProfissao").data('url'),
                dataType: 'json',
                data: {
                    "tipoConsulta": "todos",
                    "filtro": "",
                },
                success: function (data) {
                    $('#tabelaProfissao').empty();
                    $('#tabelaProfissao').append(data);
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }


        // Limpando o valor que está no campo descrição.
        $('#descricaoModalProfissao').val(""),

        // Abrindo o modal.
        // E os identificadores dos modais devem ser "modal-[Nome do Modal]"
        $('#modalProfissao').modal('show');

        // Requisição ajax passando como paramêtros os campo de tipo de consulta e descrição.
        // Formulários que porventura aparecem dentro de um modal: "form-[Nome do Modal]"
        $('#formProfissao').off('submit').submit(function (e) {
            e.preventDefault();
            $.ajax({
                type: 'POST',
                url: $(this).data('url'),
                dataType: 'json',
                data: {
                    // Campos que estão dentro de formulários em modais: "[Nome do Campo]-[Nome do Modal]"
                    "tipoConsulta": $('#tipoConsultaModalProfissao').val(),
                    "filtro": $('#descricaoModalProfissao').val(),
                },
                success: function (data) {
                    // Limpando todas as linhas que estão na tabela.
                    $('#tabelaProfissao').empty();
                    // Inserido o Html gerado no controlador no corpo da tabela.
                    $('#tabelaProfissao').append(data);

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