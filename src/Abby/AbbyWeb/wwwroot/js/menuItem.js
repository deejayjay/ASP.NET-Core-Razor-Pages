let dataTable;

$(document).ready(function () {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/MenuItem",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "25%" },
            { "data": "price", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            { "data": "foodType.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="w-100 btn-group">
                                <a href="/Admin/MenuItems/Upsert?id=${data}" class="btn btn-success text-white mx-2">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                                <a onClick="deleteMenuItem('/api/MenuItem/${data}')" class="btn btn-danger text-white mx-2">
                                    <i class="bi bi-trash-fill"></i>
                                </a>
                            </div>`;
                },
                "width": "30%"
            },
        ],
        "width": "100%"
    });
});

function deleteMenuItem(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(url, {
                method: "DELETE"
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        dataTable.ajax.reload();
                        // success notification
                        toastr.success(data.message);
                    } else {
                        // failure notification
                        toastr.error(data.message);
                    }
                })
                .catch(err => console.log(err.message));
            //Swal.fire(
            //    'Deleted!',
            //    'Your file has been deleted.',
            //    'success'
            //)
        }
    })
}