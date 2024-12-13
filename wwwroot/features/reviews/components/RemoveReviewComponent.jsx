import React from 'react';
import { useReviews } from '../hooks/useReviews';
import { removeReview } from '../redux/reviewSlice';
import { useDispatch } from 'react-redux';

const RemoveReviewComponent = () => {

    const dispatch = useDispatch();
    const { productId } = useReviews();

    const handleRemove = () => {
        dispatch(removeReview(productId));
    };

    return (
        <>

            <button className="remove-review-button" onClick={handleRemove}>
                <i className="fa-solid fa-trash-can"></i>
            </button>

        </>
    );
};

export default RemoveReviewComponent;