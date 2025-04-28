import {useDispatch} from 'react-redux';
import {useEffect, useRef} from 'react';
import {fetchUsers, setIsPageReset, setPageNumber} from '../redux/userPageSlice';
import {useUsers} from './useUsers';


const useFetchUsers = () => {
    const dispatch = useDispatch();
    const {pageNumber, pageSize, sortOrder, timePeriodFilter, filter, isLoaded, isPageReset, query} = useUsers();

    const prevSortOrder = useRef(sortOrder);
    const prevQuery = useRef(prevQuery);

    const dispatchFetchUsers = () => {
        dispatch(fetchUsers({
            pageNumber, pageSize, sortOrder, timePeriodFilter, filter, query
        }));
    }

    useEffect(() => {


        if (!isLoaded) {
            dispatchFetchUsers();
        } else if (pageNumber > 1 && (sortOrder !== prevSortOrder.current || query !== prevQuery.current) && !isPageReset) {
            dispatch(setPageNumber(1));
            dispatch(setIsPageReset(true));
        } else if (!isPageReset || (isPageReset && pageNumber === 1)) {

            dispatchFetchUsers();

            if (isPageReset && pageNumber === 1) {
                dispatch(setIsPageReset(false));
            }
        }

        prevSortOrder.current = sortOrder;
        prevQuery.current = query;
    }, [pageNumber, pageSize, sortOrder, timePeriodFilter, filter, query]);
};

export default useFetchUsers;