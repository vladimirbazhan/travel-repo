var ViewModel = function () {
    var booksUri = '/api/ver2/BooksNew/';
    var self = this;

    self.books = ko.observableArray();
    self.error = ko.observable();
    self.detail = ko.observable();

    self.getBookDetail = function (item) {
        ajaxHelper(booksUri + item.Id, 'GET').done(function (data) {
            self.detail(data);
        });
    }

    // Perform an asynchronous HTTP (Ajax) request
    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    function getAllBooks() {
        ajaxHelper(booksUri, 'GET').done(function (data) {
            self.books(data);
        });
    }

    // Fetch the initial data.
    getAllBooks();
};

ko.applyBindings(new ViewModel());
