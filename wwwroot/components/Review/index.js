import React, { useEffect } from 'react';
import { createRoot } from 'react-dom/client';
import ProductReviewsApp from './ProductReviewsApp';
import ReviewsCountApp from './ReviewsCountApp';
import ReviewsAvgRateApp from './ReviewsAvgRateApp';
import AddReviewApp from './AddReviewApp';
import UpdateReviewApp from './UpdateReviewApp';
import RemoveReviewApp from './RemoveReviewApp';
import { ReviewProvider } from './ReviewContext';

const renderInRoot = (elementId, Component) => {
    const rootElement = document.getElementById(elementId);
    if (rootElement) {
        const root = createRoot(rootElement);
        root.render(
            <ReviewProvider>
                <Component />
            </ReviewProvider>
        );
    }
};

renderInRoot('product-reviews-root', ProductReviewsApp);
renderInRoot('reviews-count-root', ReviewsCountApp);
renderInRoot('reviews-avg-rate-root', ReviewsAvgRateApp);
renderInRoot('add-review-root', AddReviewApp);
renderInRoot('update-review-root', UpdateReviewApp);
renderInRoot('remove-review-root', RemoveReviewApp);
