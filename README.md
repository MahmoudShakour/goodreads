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

```
comment has the following attributes:
- id
- userId
- reviewId
- content
- createdAt

api/comment/user/{userId}          [GET]     get comments of specific user
api/comment/review/{reviewId}      [GET]     get comments of specific review
api/comment/{commentId}            [GET]     get comment by comment id
api/comment/review/{reviewId}      [POST]    create a comment to a review
api/comment/{commentId}            [DELETE]  delete a comment by id
api/comment                        [UPDATE]  update specific comment

```

## likes

- all users can create/get likes.
- only user who created the like can delete it.

```
/api/like/user/{userId}         [GET]     get likes of specific user
/api/like/review/{reviewId}     [GET]     get likes of specific review
/api/like/review/{reviewId}     [POST]    create a like to a specific review
/api/like/review/{reviewId}     [DELETE]  delete a like from a specific review
```
