import React, { useContext, useEffect} from 'react';
import { ReviewContext } from './ReviewContext';
import UpdateReviewComponent from './UpdateReviewComponent';


const UpdateReviewApp = ({ review }) => {
    const { UpdateReview} = useContext(ReviewContext);
 
    return (
        <UpdateReviewComponent UpdateReview={UpdateReview} review={review} />
    );
};

export default UpdateReviewApp;