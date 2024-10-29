import React from 'react';
import { createPortal } from 'react-dom';
import ProductReviewsApp from './ProductReviewsApp';
import ReviewsCountApp from './ReviewsCountApp';
import ReviewsAvgRateApp from './ReviewsAvgRateApp';
import AddReviewApp from './AddReviewApp';
import UpdateReviewApp from './UpdateReviewApp';
import RemoveReviewApp from './RemoveReviewApp';


const productReviewsRoot = document.getElementById('product-reviews-root');
const reviewsCountRoot = document.getElementById('reviews-count-root');
const reviewsAvgRateRoot = document.getElementById('reviews-avg-rate-root');
const addReviewRoot = document.getElementById('add-review-root');
const updateReviewRoot = document.getElementById('update-review-root');
const removeReviewwRoot = document.getElementById('remove-review-root');


const ReviewAppWithPortals = () => (
    <>
        {productReviewsRoot && createPortal(<ProductReviewsApp />, productReviewsRoot)}
        {reviewsCountRoot && createPortal(<ReviewsCountApp />, reviewsCountRoot)}
        {reviewsAvgRateRoot && createPortal(<ReviewsAvgRateApp />, reviewsAvgRateRoot)}
        {addReviewRoot && createPortal(<AddReviewApp />, addReviewRoot)}
        {updateReviewRoot && createPortal(<UpdateReviewApp />, updateReviewRoot)}
        {removeReviewwRoot && createPortal(<RemoveReviewApp />, removeReviewwRoot)}
        
    </>
);
export default ReviewAppWithPortals;