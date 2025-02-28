import React, { Suspense, lazy } from 'react';
import { Provider } from 'react-redux';
import { createRoot } from 'react-dom/client';


const PageSizeSelectorComponent = lazy(() => import('./components/PageSizeSelectorComponent'));
const PaginationComponent = lazy(() => import('./components/PaginationComponent'));
const UsersListComponent = lazy(() => import('./components/UsersListComponent'));
const SortComponent = lazy(() => import('./components/SortComponent'));
const TotalUsersInfoComponent = lazy(() => import('./components/TotalUsersInfoComponent'));
const FilterComponent = lazy(() => import('./components/FilterComponent'));
const TimePeriodFilterComponent = lazy(() => import('./components/TimePeriodFilterComponent'));
const AdminSearchBarComponent = lazy(() => import('./components/AdminSearchBarComponent'));

export const userPageApp = (store) => {
    const portals = [
        { id: 'user-page-size-root', component: <PageSizeSelectorComponent /> },
        { id: 'user-pagination-root', component: <PaginationComponent /> },
        { id: 'user-sort-root', component: <SortComponent /> },
        { id: 'user-total-info-root', component: <TotalUsersInfoComponent /> },
        { id: 'user-filter-root', component: <FilterComponent /> },
        { id: 'user-time-period-filter-root', component: <TimePeriodFilterComponent /> },
        { id: 'user-list-root', component: <UsersListComponent /> },
        { id: 'user-search-bar-root', component: <AdminSearchBarComponent /> },
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