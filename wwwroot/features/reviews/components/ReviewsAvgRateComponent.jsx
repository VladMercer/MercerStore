import React from 'react';
import { useReviews } from '../hooks/useReviews';


const ReviewsAvgRateComponent = () => {
    const { avgReviewRate } = useReviews();

    const stars = [1, 2, 3, 4, 5].map((i) => (
      
            <i
                key={i}
                className={`fa-solid fa-star ${i <= avgReviewRate ? 'active' : ''}`}
            ></i>
        
    ));

    return <div className="stars-container">  <h5>{stars}</h5></div>;  
};

export default ReviewsAvgRateComponent;