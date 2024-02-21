# goodreads

## authentication

- user can register and log in

## books

- only admins can create/delete/update books.
- users can get all books or one book by id or isbn.
- a book has the following: id,isbn,name,year,rating,genre,numberOfPages

## authors

- only admins can CRUD authors.

## reviews

- users can create/get reviews.
- only user who created the review can update/delete it.

```
a review ,made by a user on a book, has the following attributes:

content
createdAt
it would have the following API:

/api/book/{bookId}/review [POST]    user create a review for a book
/api/review/{bookId}/review [GET]     user get all reviews of a book
/api/review/{id} [DELETE]           user delete his review
/api/review/{id} [PUT]              user update his review
/api/review/{id} [GET]              user get specific review
```

## rating

- users can CRUD their rates on books.

```
/api/rating/book/{bookId}    [GET]      get all ratings of a book
/api/rating/user/{userId}    [GET]      get all ratings of a user

/api/rating/book/{bookId}    [POST]     create a rating for a book
/api/rating/{ratingId}       [PUT]      user update his rating
/api/rating/{ratingId}       [DELETE]   user delete his rating
```

## comments

- all users can create/get comments.
- only user who created the comment can update/delete it.

## likes

- all users can create/get likes.
- only user who created the like can delete it.

## more to do

- authors has quotes that can be liked by users.
- users can follow each other.
