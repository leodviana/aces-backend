function SomenteNumero(e) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
}

//Funcao do V d grid de pesquisa
function btnConfirmarModalPessoa(codigo, nome) {
    $('#modalPessoa').modal('hide');
    $('#CodigoPessoa').val(codigo);
   $('#NomePessoa').val(nome);
}

//Funcao "OnExit" do codigo
$(function () {
    $('#CodigoPessoa').blur(function (e) {
        $('#NomePessoa').val("");

        var codigo = $(this).val().trim();
        if (codigo.length > 0) {

            $.ajax({
                type: 'GET',
                url: $(this).data('url'),
                
                data: { "codigo": codigo },
                success: function(data) {
                    if (data.encontrado=="F") {
                        alert("Pessoa não encontrada.");
                        $('#NomePessoa').val(data.nome);
                        $('#email').val(data.email);
                        $('#email2').val(data.email2);
                        $('#fone01').val(data.fone01);
                        $('#fone02').val(data.fone02);
                        $('#contato').val(data.contato);
                        $('#NumeroNotaFiscal').focus();
                        $(this).focus();
                    } else {
                       
                        $('#NomePessoa').val(data.nome);
                        $('#email').val(data.email);
                        $('#fone01').val(data.fone01);
                        $('#fone02').val(data.fone02);
                        $('#email2').val(data.email2);
                        $('#contato').val(data.contato);
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
    $('#tipoConsultaModalPessoa').change(function (e) {
        var tipo = $(this).val();
        if (tipo == "codigo") {
            $("#descricaoModalPessoa").attr('onkeypress', 'return SomenteNumero();');
           
        } else if (tipo == "descricao") {
            $("#descricaoModalPessoa").removeAttr('onkeypress');
           
        }
        $('#descricaoModalPessoa').val("");
        $('#descricaoModalPessoa').focus();

    });

    //Funcao de do botao pesquisar do modal
    // Identificadores de botões que abrem modal: "open-[Nome do Modal]"
    $('#open-Pessoa').off('click').click(function () {

        if (!$('#modalPessoa').data('carregadefault')) {
            // Limpando todas as linhas que estão na tabela.
            // Todos os identificadores de tabelas devem ser únicos, e seguir a nomeclatura "tabela-[Nome do Modal]".
            $('#tabelaPessoa').empty();
            

        }
        else //preenchendo previamento a os dados
        {
            
            $.ajax({
                type: 'POST',
                url: $("#formPessoa").data('url'),
                dataType: 'json',
                data: {
                    "tipoConsulta": "todos",
                    "filtro": "",
                },
                success: function (data) {
                    $('#tabelaPessoa').empty();
                    $('#tabelaPessoa').append(data);
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }


        // Limpando o valor que está no campo descrição.
        $('#descricaoModalPessoa').val(""),

        // Abrindo o modal.
        // E os identificadores dos modais devem ser "modal-[Nome do Modal]"
        $('#modalPessoa').modal('show');

        // Requisição ajax passando como paramêtros os campo de tipo de consulta e descrição.
        // Formulários que porventura aparecem dentro de um modal: "form-[Nome do Modal]"
        $('#formPessoa').off('submit').submit(function (e) {
            e.preventDefault();
            $.ajax({
                type: 'POST',
                url: $(this).data('url'),
                dataType: 'json',
                data: {
                    // Campos que estão dentro de formulários em modais: "[Nome do Campo]-[Nome do Modal]"
                    "tipoConsulta": $('#tipoConsultaModalPessoa').val(),
                    "filtro": $('#descricaoModalPessoa').val(),
                },
                success: function (data) {
                    // Limpando todas as linhas que estão na tabela.
                    $('#tabelaPessoa').empty();
                    // Inserido o Html gerado no controlador no corpo da tabela.
                    $('#tabelaPessoa').append(data);

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