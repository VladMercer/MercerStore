import React from 'react';

const ReviewsCountComponent = ({ countReviews }) => {
    const getReviewText = (count) => {
        if (count % 10 === 1 && count % 100 !== 11) return 'оценка';
        if (count % 10 >= 2 && count % 10 <= 4 && (count % 100 < 10 || count % 100 >= 20)) return 'оценки';
        return 'оценок';
    };
    return (
        <div className="count-reviews-text">
            {countReviews !== 0 ? (
                <span className="count-reviews">
                    {countReviews} {getReviewText(countReviews)}
                </span>
            ) : (
                <span className="no-reviews">Оценок ещё нет</span>
            )}
        </div>
    );
}

export default ReviewsCountComponent;