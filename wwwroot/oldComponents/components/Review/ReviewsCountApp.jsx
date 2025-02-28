import React, { useContext } from 'react';
import { ReviewContext } from './ReviewContext';
import ReviewsCountComponent from './ReviewsCountComponent';

const ReviewsCountApp = () => {
    const { countReviews } = useContext(ReviewContext);

    return (
        <ReviewsCountComponent countReviews={countReviews} />
    );
};

export default ReviewsCountApp;