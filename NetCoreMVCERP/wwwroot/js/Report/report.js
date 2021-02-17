window.onload = function () {
    $('#myTable').DataTable({
        //"paging": false,
        //"order": [[0, "desc"]],
        //"info": false
        //dom: 'Bfrtip',
        buttons: [
            'copy', 'excel', 'pdf'
        ]
    });
}