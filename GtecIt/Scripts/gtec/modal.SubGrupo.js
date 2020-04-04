function SomenteNumero(e) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
}

//Funcao do V d grid de pesquisa
function btnConfirmarModalSubGrupo(codigo, nome) {
    $('#modalSubGrupo').modal('hide');
    $('#CodigoSubGrupo').val(codigo);
   $('#NomeSubGrupo').val(nome);
}

//Funcao "OnExit" do codigo
$(function () {
    $('#CodigoSubGrupo').blur(function (e) {
        $('#NomeSubGrupo').val("");

        var codigo = $(this).val().trim();
        if (codigo.length > 0) {
            
            $.ajax({
                type: 'GET',
                url: $(this).data('url'),
                data: { "codigo": codigo },
                success: function(data) {
                    if (!data) {
                        alert("SubGrupo não encontrado.");
                        $(this).focus();
                    } else {
                        $('#NomeSubGrupo').val(data);
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
    $('#tipoConsultaModalSubGrupo').change(function (e) {
        var tipo = $(this).val();
        if (tipo == "codigo") {
            $("#descricaoModalSubGrupo").attr('onkeypress', 'return SomenteNumero();');
           
        } else if (tipo == "descricao") {
            $("#descricaoModalSubGrupo").removeAttr('onkeypress');
           
        }
        $('#descricaoModalSubGrupo').val("");
        $('#descricaoModalSubGrupo').focus();

    });

    //Funcao de do botao pesquisar do modal
    // Identificadores de botões que abrem modal: "open-[Nome do Modal]"
    $('#open-SubGrupo').off('click').click(function () {

        if (!$('#modalSubGrupo').data('carregadefault')) {
            // Limpando todas as linhas que estão na tabela.
            // Todos os identificadores de tabelas devem ser únicos, e seguir a nomeclatura "tabela-[Nome do Modal]".
            $('#tabelaSubGrupo').empty();
            

        }
        else //preenchendo previamento a os dados
        {
            
            $.ajax({
                type: 'POST',
                url: $("#formSubGrupo").data('url'),
                dataType: 'json',
                data: {
                    "tipoConsulta": "todos",
                    "filtro": "",
                },
                success: function (data) {
                    $('#tabelaSubGrupo').empty();
                    $('#tabelaSubGrupo').append(data);
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }


        // Limpando o valor que está no campo descrição.
        $('#descricaoModalSubGrupo').val(""),

        // Abrindo o modal.
        // E os identificadores dos modais devem ser "modal-[Nome do Modal]"
        $('#modalSubGrupo').modal('show');

        // Requisição ajax passando como paramêtros os campo de tipo de consulta e descrição.
        // Formulários que porventura aparecem dentro de um modal: "form-[Nome do Modal]"
        $('#formSubGrupo').off('submit').submit(function (e) {
            e.preventDefault();
            $.ajax({
                type: 'POST',
                url: $(this).data('url'),
                dataType: 'json',
                data: {
                    // Campos que estão dentro de formulários em modais: "[Nome do Campo]-[Nome do Modal]"
                    "tipoConsulta": $('#tipoConsultaModalSubGrupo').val(),
                    "filtro": $('#descricaoModalSubGrupo').val(),
                },
                success: function (data) {
                    // Limpando todas as linhas que estão na tabela.
                    $('#tabelaSubGrupo').empty();
                    // Inserido o Html gerado no controlador no corpo da tabela.
                    $('#tabelaSubGrupo').append(data);

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