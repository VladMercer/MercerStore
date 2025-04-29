import React, {lazy, Suspense} from 'react';
import {Provider} from 'react-redux';
import {createRoot} from 'react-dom/client';

const ProductReviewsComponent = lazy(() => import('./components/ProductReviewsComponent'));
const ReviewsCountComponent = lazy(() => import('./components/ReviewsCountComponent'));
const ReviewsAvgRateComponent = lazy(() => import('./components/ReviewsAvgRateComponent'));
const AddReviewComponent = lazy(() => import('./components/AddReviewComponent'));
const UpdateReviewComponent = lazy(() => import('./components/UpdateReviewComponent'));
const RemoveReviewComponent = lazy(() => import('./components/RemoveReviewComponent'));

export const reviewsApp = (store) => {
    const portals = [
        {id: 'product-reviews-root', component: <ProductReviewsComponent/>},
        {id: 'reviews-count-root', component: <ReviewsCountComponent/>},
        {id: 'reviews-avg-rate-root', component: <ReviewsAvgRateComponent/>},
        {id: 'add-review-root', component: <AddReviewComponent/>},
        {id: 'update-review-root', component: <UpdateReviewComponent/>},
        {id: 'remove-review-root', component: <RemoveReviewComponent/>},
    ];

    portals.forEach(({id, component}) => {
        const rootElement = document.getElementById(id);
        if (rootElement) {
            const root = createRoot(rootElement);
            root.render(
                <Provider store={store}>
                    <Suspense fallback={<div>Loading...</div>}>
                        {component}
                    </Suspense>
                </Provider>
            );
        }
    });
};