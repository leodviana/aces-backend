function SomenteNumero(e) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
}


//Funcao "OnExit" do codigo
$(function () {
    $('#CodigoProduto').blur(function (e) {
        $('#NomeProduto').val("");
        var codigo = $(this).val().trim();
        if (codigo.length > 0) {
            
            $.ajax({
                type: 'GET',
                url: $(this).data('url'),
                
                data: { "codigo": codigo },
                success: function(data) {
                    if (!data) {
                        alert("Produto não encontrado.");
                        $(this).focus();
                    } else {
                        $('#NomeProduto').text(data);
                        $('#NomeProduto').focus();
                    }
                },
                error: function(xhr, status, error) {
                    alert(error);
                }
            });
        }
    });

    //Funcao change do dropdowntipo do modal de consulta
  

 });