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
            modal.modal('show').css('z-index', 9999);
        })
        .catch(function (error) {
            console.log(error);
        });
};

function openRequestToCahngeModal(parameter) {
    
    const modal = $('#modal');

    if (parameter === undefined) {
        alert('Error')
        return;
    }

    axios.get(parameter.url)
        .then(function (response) {
            modal.find(".modal-body").html(response.data);
            modal.modal('show').css('z-index', 9999);
        })
        .catch(function (error) {
            console.log(error);
        });
};

function openModalSearch(parameter) {

    const modal = $('#modal');
    modal

    if (parameter === undefined) {
        alert('Error')
        return;
    }

    axios.get(parameter.url)
        .then(function (response) {
            modal.find(".modal-body").html(response.data);
            modal.modal('show').css('z-index', 9999);
        })
        .catch(function (error) {
            console.log(error);
        });
};