import React from 'react';

const RemoveReviewComponent = ({RemoveReview, productId}) => {

    const handleRemove = () => {
        RemoveReview(productId);
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