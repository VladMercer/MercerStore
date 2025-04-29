import {useDispatch} from 'react-redux';
import {useEffect, useRef} from 'react';
import {fetchResults, setIsPageReset, setPageNumber, setQuery} from '../redux/searchPageSlice';
import {useSearchPage} from './useSearchPage';


const useFetchSearchPage = () => {
    const dispatch = useDispatch();
    const {query, pageNumber, pageSize, sortOrder, isLoaded, isPageReset} = useSearchPage();

    const prevSortOrder = useRef(sortOrder);
    const prevPageSize = useRef(pageSize);

    const dispatchFecthResults = () => {
        dispatch(fetchResults({
            query, sortOrder, pageNumber, pageSize
        }));

    }

    useEffect(() => {
        const searchParams = new URLSearchParams(window.location.search);
        const query = searchParams.get('query');
        dispatch(setQuery(query));

    }, []);


    useEffect(() => {

        if (query) {

            if (!isLoaded) {
                dispatchFecthResults();

            } else if (pageNumber > 1 && (sortOrder !== prevSortOrder.current ||
                prevPageSize.current !== pageSize) && !isPageReset) {
                dispatch(setPageNumber(1));
                dispatch(setIsPageReset(true));

            } else if (!isPageReset || (isPageReset && pageNumber === 1)) {

                dispatchFecthResults();

                if (isPageReset && pageNumber === 1) {
                    dispatch(setIsPageReset(false));
                }
            }

            prevSortOrder.current = sortOrder;
            prevPageSize.current = pageSize;
        }

    }, [query, pageNumber, pageSize, sortOrder]);
};

export default useFetchSearchPage;