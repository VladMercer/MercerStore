import { useSelector } from 'react-redux';

export const useSearchPage = () => {


    const results = useSelector((state) => state.searchPage.results);
    const pageNumber = useSelector((state) => state.searchPage.pageNumber);
    const pageSize = useSelector((state) => state.searchPage.pageSize);
    const totalPages = useSelector((state) => state.searchPage.totalPages);
    const sortOrder = useSelector((state) => state.searchPage.sortOrder);
    const isLoaded = useSelector((state) => state.searchPage.isLoaded);
    const isPageReset = useSelector((state) => state.searchPage.isPageReset);
    const query = useSelector((state) => state.searchPage.query);
    const totalItems = useSelector((state) => state.searchPage.totalItems);

    return {
        results,
        pageNumber,
        pageSize,
        totalPages,
        sortOrder,
        isLoaded,
        isPageReset,
        query,
        totalItems
        
    };
};