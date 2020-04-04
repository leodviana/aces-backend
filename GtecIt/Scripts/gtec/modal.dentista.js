function SomenteNumero(e) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
}

//Funcao do V d grid de pesquisa
function btnConfirmarModalDentista(codigo, nome) {
    
    $('#modalDentista').modal('hide');
    $('#CodigoDentista').val(codigo);
    $('#NomeDentista').val(nome);
}

//Funcao "OnExit" do codigo
$(function () {
    $('#CodigoDentista').blur(function (e) {
        $('#NomeDentista').val("");
        
        var codigo = $(this).val().trim();
        
        if (codigo.length > 0) {

            $.ajax({
                type: 'GET',
                url: $(this).data('url'),

                data: { "codigo": codigo },
                success: function (data) {
                    
                    if (!data) {
                        alert("Dentista não encontrado.");
                        $(this).focus();
                    } else {
                        $('#NomeDentista').val(data);
                        $('#NomeDentista').focus();
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }
    });

    //Funcao change do dropdowntipo do modal de consulta
    $('#tipoConsultaModalDentista').change(function (e) {
        var tipo = $(this).val();
        if (tipo == "codigo") {
            $("#descricaoModalDentista").attr('onkeypress', 'return SomenteNumero();');

        } else if (tipo == "descricao") {
            $("#descricaoModalDentista").removeAttr('onkeypress');

        }
        $('#descricaoModalDentista').val("");
        $('#descricaoModalDentista').focus();

    });

    //Funcao de do botao pesquisar do modal
    // Identificadores de botões que abrem modal: "open-[Nome do Modal]"
    $('#open-Dentista').off('click').click(function () {

        if (!$('#modalDentista').data('carregadefault')) {
            // Limpando todas as linhas que estão na tabela.
            // Todos os identificadores de tabelas devem ser únicos, e seguir a nomeclatura "tabela-[Nome do Modal]".
            $('#tabelaDentista').empty();


        }
        else //preenchendo previamento a os dados
        {

            $.ajax({
                type: 'POST',
                url: $("#formDentista").data('url'),
                dataType: 'json',
                data: {
                    "tipoConsulta": "todos",
                    "filtro": "",
                },
                success: function (data) {
                    $('#tabelaDentista').empty();
                    $('#tabelaDentista').append(data);
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }


        // Limpando o valor que está no campo descrição.
        $('#descricaoModalDentista').val(""),

        // Abrindo o modal.
        // E os identificadores dos modais devem ser "modal-[Nome do Modal]"
        $('#modalDentista').modal('show');

        // Requisição ajax passando como paramêtros os campo de tipo de consulta e descrição.
        // Formulários que porventura aparecem dentro de um modal: "form-[Nome do Modal]"
        $('#formDentista').off('submit').submit(function (e) {
            e.preventDefault();
            $.ajax({
                type: 'POST',
                url: $(this).data('url'),
                dataType: 'json',
                data: {
                    // Campos que estão dentro de formulários em modais: "[Nome do Campo]-[Nome do Modal]"
                    "tipoConsulta": $('#tipoConsultaModalDentista').val(),
                    "filtro": $('#descricaoModalDentista').val(),
                },
                success: function (data) {
                    // Limpando todas as linhas que estão na tabela.
                    $('#tabelaDentista').empty();
                    // Inserido o Html gerado no controlador no corpo da tabela.
                    $('#tabelaDentista').append(data);

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