/* Workers capped at 4 instead of processor count: these UI tests now drive
 *  multi-page flows (registration, checkout, payment) against a live
 * third-party site instead of the old single page demo widgets and running
 * too many Chrome sessions at once causes resource contention and spurious
 * timeouts rather than any faster wall-clock time */
[assembly: Parallelize(Workers = 4, Scope = ExecutionScope.MethodLevel)]
