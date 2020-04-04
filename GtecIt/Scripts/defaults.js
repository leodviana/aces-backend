$(function () {
    $('.datetimepicker').inputmask('99/99/9999');
    $('.datetimepicker').blur(function () {
        var dateWithoutMask = $(this).val().replace(/[^\d]+/g, '');

        if (dateWithoutMask.length < 8) {
            $(this).val("");
        } else {
            var datepattern = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[0-9]|[0-9]\d)?\d{2})$/;

            if (!datepattern.test($(this).val())) {
                $(this).val("");
                alert("Data informada inválida!\nDatas devem seguir o formato: DIA/MÊS/ANO.");

            }
        }
    });

    $('.datesimple').inputmask('99/99/9999');
    $('.datesimple').blur(function () {
        var dateWithoutMask = $(this).val().replace(/[^\d]+/g, '');

        if (dateWithoutMask.length < 8) {
            $(this).val("");
        } else {
            var datepattern = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[0-9]|[0-9]\d)?\d{2})$/;

            if (!datepattern.test($(this).val())) {
                $(this).val("");
                alert("Data informada inválida!\nDatas devem seguir o formato: DIA/MÊS/ANO.");

            }
        }
    });


    $(".datetimepicker").datetimepicker({
        format: 'DD/MM/YYYY',
        locale: 'pt-BR'
    });

    $('.dinheiro').maskMoney({
        decimal: ",",
        thousands: ".",
        allowZero: true,
        defaultZero: true
    });

    $('.fone').inputmask('(99)9999-9999#');

    function SomenteNumero(e) {
        var tecla = (window.event) ? event.keyCode : e.which;
        if ((tecla > 47 && tecla < 58)) return true;
        else {
            if (tecla == 8 || tecla == 0) return true;
            else return false;
        }
    }
});