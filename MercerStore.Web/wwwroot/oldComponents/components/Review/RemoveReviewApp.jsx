import React, {useContext} from 'react';
import {ReviewContext} from './ReviewContext';
import RemoveReviewComponent from './RemoveReviewComponent';


const RemoveReviewApp = () => {
    const {RemoveReview, productId} = useContext(ReviewContext);

    return (
        <RemoveReviewComponent RemoveReview={RemoveReview} productId={productId}/>
    );
};

export default RemoveReviewApp;