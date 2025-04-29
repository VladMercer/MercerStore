import React, {lazy, Suspense} from 'react';
import {Provider} from 'react-redux';
import {createRoot} from 'react-dom/client';

const ResultsTableComponent = lazy(() => import('./components/ResultsTableComponent'));
const AdminSearchBarComponent = lazy(() => import('./components/AdminSearchBarComponent'));

export const adminSearchPageApp = (store) => {
    const portals = [
        {id: 'admin-search-result-table-root', component: <ResultsTableComponent/>},
        {id: 'admin-search-bar-root', component: <AdminSearchBarComponent/>},
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