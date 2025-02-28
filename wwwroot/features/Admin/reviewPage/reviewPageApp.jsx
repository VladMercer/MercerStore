import React, { Suspense, lazy } from 'react';
import { Provider } from 'react-redux';
import { createRoot } from 'react-dom/client';


const PageSizeSelectorComponent = lazy(() => import('./components/PageSizeSelectorComponent'));
const PaginationComponent = lazy(() => import('./components/PaginationComponent'));
const ReviewsListComponent = lazy(() => import('./components/ReviewsListComponent'));
const SortComponent = lazy(() => import('./components/SortComponent'));
const TotalReviewsInfoComponent = lazy(() => import('./components/TotalReviewsInfoComponent'));
const FilterComponent = lazy(() => import('./components/FilterComponent'));
const TimePeriodFilterComponent = lazy(() => import('./components/TimePeriodFilterComponent'));
const AdminSearchBarComponent = lazy(() => import('./components/AdminSearchBarComponent'));

export const reviewPageApp = (store) => {
    const portals = [
        { id: 'review-page-size-root', component: <PageSizeSelectorComponent /> },
        { id: 'review-pagination-root', component: <PaginationComponent /> },
        { id: 'review-sort-root', component: <SortComponent /> },
        { id: 'review-total-info-root', component: <TotalReviewsInfoComponent /> },
        { id: 'review-filter-root', component: <FilterComponent /> },
        { id: 'review-time-period-filter-root', component: <TimePeriodFilterComponent /> },
        { id: 'review-list-root', component: <ReviewsListComponent /> },
        { id: 'review-search-bar-root', component: <AdminSearchBarComponent /> },
    ];

    portals.forEach(({ id, component }) => {
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