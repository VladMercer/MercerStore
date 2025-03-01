import React from 'react';
const ReviewsAvgRateComponent = ({ avgReviewRate }) => {
   
    const stars = [1, 2, 3, 4, 5].map((i) => (
        <i
            key={i}
            className={`fa-solid fa-star ${i <= avgReviewRate ? 'active' : ''}`} 
        ></i>
    ));

    return <div className="stars-container">{stars}</div>;  
};

export default ReviewsAvgRateComponent;