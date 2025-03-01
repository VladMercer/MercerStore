import { useSelector } from 'react-redux';

export const useSearchResults = () => {
    return useSelector((state) => ({
        results: state.adminSearchPage.results,
    }));
};
