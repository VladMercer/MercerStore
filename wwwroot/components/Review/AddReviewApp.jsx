import React, { useContext } from 'react';
import { ReviewContext } from './ReviewContext';
import AddReviewComponent from './AddReviewComponent';

const AddReviewApp = () => {
    const { AddReview, productId, review } = useContext(ReviewContext);

    const hasReview = review && review.length > 0;
    return (
        <>
            {!hasReview && (
                <AddReviewComponent AddReview={AddReview} productId={productId} />
            )}
            {hasReview && (
                <p>Вы уже оставили отзыв. Спасибо!</p>
            )}
        </>
    );
};

export default AddReviewApp;