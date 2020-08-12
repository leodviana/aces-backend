var events = [];
console.log(events);
var selectedEvent = null;

FetchEventAndRenderCalendar();
function FetchEventAndRenderCalendar() {
    events = [];
    $.ajax({
        type: "GET",
        url: '@Url.Action("GetEvents", "Events")',
       
        success: function (data) {

            $.each(data, function (i, v) {
                events.push({
                    eventID: v.EventID,
                    title: v.Subject,
                    description: v.Description,
                    start: moment(v.Start),
                    end: v.End != null ? moment(v.End) : null,
                    color: v.ThemeColor,
                    allDay: v.IsFullDay,
                    contrato: v.contrato,
                    professor: v.professor
                });
            })

            GenerateCalender(events);
        },
        error: function (error) {
            alert("failed");

        }
    })
}

function GenerateCalender(events) {
    $('#calender').fullCalendar('destroy');
    $('#calender').fullCalendar({
        lang: 'pt-br',
        contentHeight: 400,
        defaultDate: new Date(),

        timeFormat: 'h(:mm)a',

        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,basicWeek,basicDay,agenda'
        },

        eventLimit: true,

        eventColor: '#378006',
        events: events,
        eventClick: function (calEvent, jsEvent, view) {

            selectedEvent = calEvent;
            $('#myModal #eventTitle').text(calEvent.title);
            var $description = $('<div/>');
            $description.append($('<p/>').html('<b>Inicio: </b>' + calEvent.start.format("DD-MM-YYYY HH:mm a")));
            if (calEvent.end != null) {
                $description.append($('<p/>').html('<b>Termino: </b>' + calEvent.end.format("DD-MM-YYYY HH:mm a")));
            }
            if (calEvent.description != null) {
                $description.append($('<p/>').html('<b>Observação:</b>' + calEvent.description));

            }
            $('#myModal #pDetails').empty().html($description);

            $('#myModal').modal();
        },
        selectable: true,
        select: function (start, end) {
            selectedEvent = {
                eventID: 0,
                title: '',
                description: '',
                start: start,
                end: end,
                allDay: false,
                color: '',
                contrato: 0,
                professor: 0
            };
            openAddEditForm();
            $('#calendar').fullCalendar('unselect');
        },
        editable: true,
        eventDrop: function (event) {
            var data = {
                EventID: event.eventID,
                Subject: event.title,
                Start: event.start.format('DD/MM/YYYY HH:mm A'),
                End: event.end != null ? event.end.format('DD/MM/YYYY HH:mm A') : null,
                Description: event.description != null ? event.description : "",
                ThemeColor: event.color,
                IsFullDay: event.allDay,
                contrato: event.contrato,
                professor: event.profId
            };
            SaveEvent(data);
        }
    })
}

$('#btnEdit').click(function () {
    //Open modal dialog for edit event
    openAddEditForm();
})
$('#btnDelete').click(function () {
    if (selectedEvent != null && confirm('Are you sure?')) {
        $.ajax({
            type: "POST",
            url: '/Events/DeleteEvent',
            data: { 'eventID': selectedEvent.eventID },
            success: function (data) {
                if (data.status) {
                    //Refresh the calender
                    FetchEventAndRenderCalendar();
                    $('#myModal').modal('hide');
                }
            },
            error: function () {
                alert('Failed');
            }
        })
    }
})

$('#dtp1,#dtp2').datetimepicker({
    format: 'DD/MM/YYYY HH:mm A'
});


$('#departmentsDropdown').change(function () {
    var contrato = $('#departmentsDropdown option').filter(':selected').val()
    var texto = $('#departmentsDropdown :selected').text();

    $('#txtSubject').val = ("");

    $('#txtSubject').val("Contrato " + contrato + " - " + texto);


});



$('#chkIsFullDay').change(function () {
    if ($(this).is(':checked')) {
        $('#divEndDate').hide();
    }
    else {
        $('#divEndDate').show();
    }
});

function openAddEditForm() {
    if (selectedEvent != null) {

        $('#hdEventID').val(selectedEvent.eventID);
        $('#txtSubject').val(selectedEvent.title);
        $('#txtStart').val(selectedEvent.start.format('DD/MM/YYYY HH:mm A'));
        $('#chkIsFullDay').prop("checked", selectedEvent.allDay || false);
        $('#chkIsFullDay').change();
        $('#txtEnd').val(selectedEvent.end != null ? selectedEvent.end.format('DD/MM/YYYY HH:mm A') : '');
        $('#txtDescription').val(selectedEvent.description);
        $('#ddThemeColor').val(selectedEvent.color);
        $("#departmentsDropdown option").filter(':selected').val(selectedEvent.contrato)
        $("#professorDropdown option").filter(':selected').val(selectedEvent.profId)

        $.ajax({
            type: "GET",
            url: "/Events/getContratos",
            data: "{}",
            success: function (data) {
                var s = '<option value="-1">Selecione o aluno</option>';
                for (var i = 0; i < data.length; i++) {
                    if (data[i].DepartmentID == selectedEvent.contrato) {

                        s += '<option value="' + data[i].DepartmentID + '"selected>' + data[i].DepartmentName + '</option>';

                    }
                    else {
                        s += '<option value="' + data[i].DepartmentID + '">' + data[i].DepartmentName + '</option>';
                    }

                }
                $("#departmentsDropdown").html(s);
            }
        });
        $.ajax({
            type: "GET",
            url: "/Events/getProfessores",
            data: "{}",
            success: function (data) {
                var s = '<option value="-1">Selecione o professor</option>';
                for (var i = 0; i < data.length; i++) {

                    if (data[i].ProfessorID == selectedEvent.professor) {

                        s += '<option value="' + data[i].ProfessorID + '"selected>' + data[i].ProfessorName + '</option>';
                    }
                    else {
                        s += '<option value="' + data[i].ProfessorID + '">' + data[i].ProfessorName + '</option>';
                    }

                }
                $("#professorDropdown").html(s);
            }
        });
    }
    $('#myModal').modal('hide');

    $('#myModalSave').modal();
}

$('#btnSave').click(function () {

    /*if ($("#departmentsDropdown option").filter(':selected').val() <=0) {
        alert('Selecione um Aluno');
        return;
    }

    if ($('#txtSubject').val().trim() == "") {
        alert('Subject required');
        return;
    }
    if ($('#txtStart').val().trim() == "") {
        alert('Start date required');
        return;
    }
    if ($('#chkIsFullDay').is(':checked') == false && $('#txtEnd').val().trim() == "") {
        alert('End date required');
        return;
    }
    else {
        var startDate = moment($('#txtStart').val(), "DD/MM/YYYY HH:mm A").toDate();
        var endDate = moment($('#txtEnd').val(), "DD/MM/YYYY HH:mm A").toDate();
        if (startDate > endDate) {
            alert('Invalid end date');
            return;
        }
    }*/

    var data = {
        EventID: $('#hdEventID').val(),
        Subject: $('#txtSubject').val().trim(),
        Start: $('#txtStart').val().trim(),
        End: $('#chkIsFullDay').is(':checked') ? null : $('#txtEnd').val().trim(),
        Description: $('#txtDescription').val(),
        ThemeColor: $('#ddThemeColor').val(),
        IsFullDay: $('#chkIsFullDay').is(':checked'),
        contrato: $('#departmentsDropdown option').filter(':selected').val(),
        professor: $('#professorDropdown option').filter(':selected').val()
    }
    SaveEvent(data);
    // call function for submit data to the server
})

function SaveEvent(data) {
    $.ajax({
        type: "POST",
        url: '/Events/SaveEvent',
        data: data,
        success: function (data) {
            if (data.Sucesso == true) {
                //Refresh the calender
                FetchEventAndRenderCalendar();
                $('#myModalSave').modal('hide');
            }
            else {
                $("#divErros").empty();

                var i = 0;
                $(data.msg).each(function () {


                    $("#divErros").append("<p style=color:red>" + "* " + data.msg[i] + "</p>");

                    i = i + 1;

                    //crie uma divErros em sua modal.
                });


            }
        },
        error: function () {
            alert('Failed');
        }
    })
}