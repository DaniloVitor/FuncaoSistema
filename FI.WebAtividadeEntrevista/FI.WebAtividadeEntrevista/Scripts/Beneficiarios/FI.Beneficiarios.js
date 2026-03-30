$(document).on("submit", "#formBeneficiario", function (e) {
    e.preventDefault();
    var form = $(this);

    $.ajax({
        url: form.attr("action"),
        method: "POST",
        data: form.serialize(),
        success: function (html) {
            $("#containerTabela").html(html);
            form[0].reset();
            form.attr("action", "/Beneficiario/Incluir");
            $("#IdBeneficiario").val("");
            form.find("button[type='submit']").text("Incluir");
        },
        error: function (r) {
            if (r.status == 400) {
                ModalDialog("Erro", r.responseJSON);
            }
            else if (r.status == 500)
                ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            else {
                ModalDialog("Erro", "Erro inesperado.");
            }
        }
    });
});

$(document).on("click", ".btn-editar", function () {
    const id = $(this).data("id");
    const linha = $(this).closest("tr");
    const cpf = linha.find("td:eq(0)").text();
    const nome = linha.find("td:eq(1)").text();

    $("#CPFBeneficiario").val(cpf);
    $("#NomeBeneficiario").val(nome);
    $("#IdBeneficiario").val(id);

    $("#formBeneficiario").attr("action", "/Beneficiario/Alterar");
    $("#formBeneficiario button[type='submit']").text("Alterar");
});

$(document).on("click", ".btn-excluir", function () {
    const id = $(this).data("id");

    $.ajax({
        url: '/Beneficiario/Excluir',
        type: 'POST',
        data: { id: id },
        success: function (html) {
            $("#containerTabela").html(html);
        },
        error: function () {
            ModalDialog("Erro", "Erro inesperado.");
        }
    });
});

$(document).on("focus", "#CPFBeneficiario", function () {
    if (!$(this).data("mask-applied")) {
        $(this).mask("000.000.000-00");
        $(this).data("mask-applied", true);
    }
});

function formatarCPF() {
    $(".cpf-text").each(function () {
        let cpf = $(this).text().replace(/\D/g, "");

        if (cpf.length === 11) {
            cpf = cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
            $(this).text(cpf);
        }
    });
}

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}