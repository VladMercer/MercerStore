import React from 'react';
import RemoveReviewComponent from './RemoveReviewComponent';
import UpdateReviewComponent from './UpdateReviewComponent';
import useFetchReviews from '../hooks/useFetchReviews';
import {useReviews} from '../hooks/useReviews';

const ProductReviewsComponent = () => {
    useFetchReviews();
    const {currentUserId, productReviews} = useReviews();

    return (
        <div>

            {productReviews.map(review => (
                review && review.userName && review.date && (
                    <div key={`${review.userId}`} className="card product-review mb-3">
                        <div className="card-body">
                            <div className="product-review-header">
                                <h5 className="card-title">{review.userName}</h5>
                                <span className="review-date">{new Date(review.date).toLocaleDateString()}</span>
                            </div>
                            <div className="product-review-subheader">
                                <div className="product-rating mb-3">
                                    {[...Array(5)].map((star, i) => (
                                        <i
                                            key={i}
                                            className={`fa-solid fa-star ${i < review.value ? 'active' : ''}`}
                                        ></i>
                                    ))}
                                </div>

                                <div className="review-buttons">
                                    {currentUserId === review.userId && (
                                        <>
                                            <UpdateReviewComponent/>
                                            <RemoveReviewComponent/>
                                        </>
                                    )}
                                </div>
                            </div>
                            <div className="card-text">
                                <p>{review.reviewText}</p>
                            </div>
                        </div>
                    </div>
                )
            ))}
        </div>
    );
};

export default ProductReviewsComponent;