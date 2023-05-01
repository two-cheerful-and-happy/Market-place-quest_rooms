function openModalForm(parameters) {
    const id = parameters.data;
    const url = parameters.url;
    const modal = $('#modal');

    if (id === undefined || url === undefined) {
        alert('Error')
        return;
    }

    axios.get(url + '/' + id)
        .then(function (response) {
            modal.find(".modal-body").html(response.data);
            modal.modal('show')
        })
        .catch(function (error) {
            console.log(error);
        });
};

function openModelOnChanging(parameters) {
    const url = parameters.url;
    const modal = $('#modal');

    if (url === undefined) {
        alert('Error')
        return;
    }

    axios.get(url)
        .then(function (response) {
            modal.find(".modal-body").html(response.data);
            modal.modal('show')
        })
        .catch(function (error) {
            console.log(error);
        });
};