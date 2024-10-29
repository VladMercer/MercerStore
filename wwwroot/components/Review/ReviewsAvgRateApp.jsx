import React, { useContext } from 'react';
import { ReviewContext } from './ReviewContext';
import ReviewsAvgRateComponent from './ReviewsAvgRateComponent';


const ReviewsAvgRateApp = () => {
    const { avgReviewRate } = useContext(ReviewContext);

    return (
        <ReviewsAvgRateComponent avgReviewRate={avgReviewRate}/>
    );
};

export default ReviewsAvgRateApp;