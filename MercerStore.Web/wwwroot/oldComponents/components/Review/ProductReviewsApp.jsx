import React, {useContext} from 'react';
import {ReviewContext} from './ReviewContext';
import ProductReviewsComponent from './ProductReviewsComponent';


const ProductReviewsApp = () => {
    const {currentUserId, productReviews, review} = useContext(ReviewContext);

    return (
        <ProductReviewsComponent productReviews={productReviews} currentUserId={currentUserId} review={review}/>
    );
};

export default ProductReviewsApp;