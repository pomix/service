$(document).ready(function () {
    var app = {
        initialize: function () {
            this.setUpListeners();
            this.createTooltip();
        },
        setUpListeners: function () {
            $('form').on('submit', app.submitForm);

            $('form').on('keydown','input', app.removeError);
        },
        createTooltip:function(){
            //var inputs = $('input');

            //$.each(inputs, function (index, val) {
            //    var input = $(val),
            //        formGroup = input.parents('.form-group');
            //        label = formGroup.find('label').text().toLowerCase(),
            //        textError = 'Введите ' + label;
            //    if (input = !$('input#MobilePhone')) {
            //        formGroup.addClass('has-error').removeClass('has-success');
            //        input.tooltip({
            //            trigger: 'manual',
            //            placement: 'bottom',
            //            title: textError
            //        })
            //    }
            //    else{
            //        input.tooltip({
            //            trigger: 'manual',
            //            placement: 'bottom',
            //            title: 'Неверный мобильный'
            //            })
            //    }
            //})
        },
        hideTooltip: function () {
            var inputs = $('input');

            $.each(inputs, function (index, val) {
                var input = $(val);
                input.tooltip('hide').parents('.form-group').removeClass('has-error').removeClass('has-success');
            })
        },
        submitForm: function (e) {
            e.preventDefault();

            var form = $(this);
            if (app.validateForm(form) === false) return false;
            else {
                
                app.createUpdateContact();
            }
        },
        validateForm: function (form) {
            var inputs = form.find('input'),
                vlid = true,
                mobil = /^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,13}$/,
                    mobilSelector = $('input#MobilePhone');
            $.each(inputs, function (index, val) {
                
                var input = $(val),
                    val = input.val(),
                    formGroup = input.parents('.form-group'),                    
                    label = formGroup.find('label').text().toLowerCase(),
                    textError = 'Введите ' + label;
                if (mobilSelector != input) {
                    if (val.length === 0) {
                        formGroup.addClass('has-error').removeClass('has-success');
                        input.tooltip({
                            trigger: 'manual',
                            placement: 'bottom',
                            title: textError
                        }).tooltip('show');
                        vlid = false;
                    }
                    else {
                        formGroup.addClass('has-success').remove('has-error');

                    }
                }
                else {
                    if (mobil.test(mobilSelector.val()) == false) {
                        mobilSelector.tooltip({
                            trigger: 'manual',
                            placement: 'bottom',
                            title: 'Неверный мобильный'
                        }).tooltip('show');
                        vlid = false;
                    }
                }
            })
            return vlid;
        },
        removeError: function () {
            $(this).tooltip('hide').parents('.form-group').removeClass('has-error');
        },
        createUpdateContact: function () {
            $.ajax({
                type: 'POST',
                url: '/Home/CreateUpdataContact',
                dataType: 'json',
                data: {
                    id: $('div.modal-body').attr('id'), Name: $('[name="Name"]').val(),
                    MobilePhone: $('[name="MobilePhone"]').val(),
                    Dear: $('[name="Dear"]').val(), JobTitle: $('[name="JobTitle"]').val(),
                    BirthDate: $('[name="BirthDate"]').val()
                },
                success: function (data) {
                    if (Number(data) == 1) {
                        console.log(data)
                        var _id = $('div.modal-body').attr('id');
                        $('tr#' + _id).html('<td>' + $('[name="Name"]').val() + '</td>' +
                                '<td>' + $('[name="MobilePhone"]').val() + '</td>' +
                                '<td>' + $('[name="Dear"]').val() + '</td>' +
                                '<td>' + $('[name="JobTitle"]').val() + '</td>' +
                                '<td>' + $('[name="BirthDate"]').val() + '</td>');
                        $('#myModal').modal('hide');
                    }
                    else if (Number(data) == 0) {
                        $('tbody').html('');
                        getContract();
                        position = 0;
                        $('#myModal').modal('hide');
                    }
                }

            })
        }

    }
    app.initialize();
    getContract()
    var position = 0;
    var _tr
    
    
    $('#datetimepicker10').datetimepicker({
        viewMode: 'years',
        format: 'DD.MM.YYYY'
    });

    $('.btn-link').on('click', function () {
        position+=40;
        $.ajax({
            type: 'GET',
            url: '/Home/GetContact',
            dataType: 'json',
            data:{position:position},
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    $('tbody').append('<tr id="' + data[i].Id + '"><td>' + data[i].Name + '</td>' +
                        '<td>' + data[i].MobilePhone + '</td>' +
                        '<td>' + data[i].Dear + '</td>' +
                        '<td>' + data[i].JobTitle + '</td>' +
                        '<td>' + data[i].BirthDate + '</td></tr>')
                    $('#' + data[i].Id).click(function () {
                        var _td = $(this).children();
                        modalInput(_td);
                        app.hideTooltip();
                        $('#myModal').modal('show');
                    })
                }
                
                
            }
        });
    })
   
    $('#create-contact').click(function () {
        $('div.modal-body').attr('id', 'NAN');
        $('[name="Name"]').val(null);
        $('[name="MobilePhone"]').val(null);
        $('[name="Dear"]').val(null);
        $('[name="JobTitle"]').val(null);
        $('[name="BirthDate"]').val(null);
        $('#myModal').modal('show');
    })
    $('#delete-contact').click(function () {

        $.ajax({
            type: 'POST',
            url: '/Home/DeleteContact',
            dataType: 'json',
            data: {
                id: $('div.modal-body').attr('id')
            },
            success: function (data) {
                console.log(data);
                if (data == 1) {
                    var tr_id = $('div.modal-body').attr('id');
                    $('tr#' + tr_id).remove();
                    $('#myModal').modal('hide');
                    position -= 1;
                }
                else {

                    $('#myModal').modal('hide');
                    alert('Выбранный элемент удалить невозможно')
                }
            }
        })
       


    })
    function getContract() {
        $.ajax({
            type: 'GET',
            url: '/Home/GetContact',
            dataType: 'json',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    $('tbody').append('<tr id="' + data[i].Id + '"><td>' + data[i].Name + '</td>' +
                        '<td>' + data[i].MobilePhone + '</td>' +
                        '<td>' + data[i].Dear + '</td>' +
                        '<td>' + data[i].JobTitle + '</td>' +
                        '<td>' + data[i].BirthDate + '</td></tr>')
                    $('#' + data[i].Id).click(function () {
                        var _td = $(this).children();
                        modalInput(_td);
                        app.hideTooltip();
                        $('#myModal').modal('show');
                    })
                }
            }
        });
    }

    function modalInput(_td)
    {
        $('div.modal-body').attr('id',($(_td[0]).parent().attr('Id')));
        $('[name="Name"]').val($(_td[0]).text());
        $('[name="MobilePhone"]').val($(_td[1]).text());
        $('[name="Dear"]').val($(_td[2]).text());
        $('[name="JobTitle"]').val($(_td[3]).text());
        $('[name="BirthDate"]').val($(_td[4]).text());
    }
})