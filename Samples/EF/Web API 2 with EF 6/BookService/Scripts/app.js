// TODO AG: Knockout.js MVVM
var ViewModel = function () {
    var booksUri = '/api/ver2/BooksNew/';
    var authorsUri = '/api/Authors/';
    var self = this;

    self.books = ko.observableArray();
    self.authors = ko.observableArray();
    self.error = ko.observable();
    self.detail = ko.observable();

    self.getBookDetail = function (item) {
        debugger;
        ajaxHelper(booksUri + item.Id, 'GET').done(function (data) {
            self.detail(data);
        });
    }

    self.newBook = {
        Author: ko.observable(),
        Genre: ko.observable(),
        Price: ko.observable(),
        Title: ko.observable(),
        Year: ko.observable()
    }

    self.addBook = function (formElement) {
        var book = {
            AuthorId: self.newBook.Author().Id,
            Genre: self.newBook.Genre(),
            Price: self.newBook.Price(),
            Title: self.newBook.Title(),
            Year: self.newBook.Year()
        };

        ajaxHelper(booksUri, 'POST', book).done(function (item) {
            debugger; 
            self.books.push(item);
        }).error(function (err) {
            debugger;
            alert(JSON.stringify(err));
        });
    }

    self.removeBook = function (item) {
        debugger;
        ajaxHelper(booksUri + item.Id, 'DELETE').done(function () {
            getAllBooks();
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

    function getAuthors() {
        ajaxHelper(authorsUri, 'GET').done(function (data) {
            self.authors(data);
        });
    }

    // Fetch the initial data.
    getAllBooks();
    getAuthors();
};

ko.applyBindings(new ViewModel());
